using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Server.Config;
using static System.FormattableString; 

namespace Server.ServerConsole
{
    public static class Logger
    {
        private static bool _liveView = true;
        
        public static bool LiveView
        {
            get => _liveView;
            set
            {
                _liveView = value;
                ConfigManager.PrimaryConfig.LiveView = value;
                ConfigManager.SavePrimary();
            }
        }
        
        private static readonly ConcurrentQueue<LogEntry> Q = new();

        private static readonly ConsoleColor[] Colors = 
            { ConsoleColor.Gray, ConsoleColor.Green, ConsoleColor.DarkMagenta, ConsoleColor.Cyan, ConsoleColor.Red, ConsoleColor.Magenta };

        internal static void Log(string text, LogEntryPriority priority = LogEntryPriority.Info, uint gameId = 0,
            LogType type = LogType.PrintAndLog) => Q.Enqueue(new LogEntry(
            priority == LogEntryPriority.LiveView
                ? $"{(gameId == uint.MaxValue ? "[-]" : $"[{gameId}]")} {text}"
                : text, priority, type));

        internal static async Task QueueTask(CancellationToken token)
        {
            if (!Directory.Exists("Logs"))
                Directory.CreateDirectory("Logs");

            var d = DateTime.UtcNow;
            uint checks = 0;

            var fs = new FileStream(Invariant($"Logs{Path.DirectorySeparatorChar}{d:yyyy-MM-ddTHH-mm-ss}.log"),
                FileMode.OpenOrCreate, FileAccess.Write,
                FileShare.ReadWrite);
            var sw = new StreamWriter(fs)
            {
                AutoFlush = true
            };
            
            Log("Started logging.");
            
            try
            {
                while (true)
                {
                    try
                    {
                        if (Q.TryDequeue(out var le))
                        {
                            if (le.Message == null)
                                continue;

                            var ts = Invariant($"[{le.Timestamp:HH:mm:ss.ff}] ");
                            if (le.Priority == LogEntryPriority.CommandInput)
                                ts += ">>> ";

                            var split = le.Message.Split('\n');
                            foreach (var l in split)
                            {
                                if (le.Type != LogType.Log && (LiveView || le.Priority != LogEntryPriority.LiveView))
                                {
                                    Console.ForegroundColor = Colors[(byte)le.Priority];
                                    Console.Write(ts);
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                    Console.WriteLine(l);
                                }

                                if (le.Type != LogType.Print)
                                    await sw.WriteLineAsync($"{ts}{l}");
                            }
                        }
                        else if (token.IsCancellationRequested)
                            break;
                        else
                        {
                            try
                            {
                                await Task.Delay(25, token);
                            }
                            catch (TaskCanceledException)
                            {
                                //Ignore
                            }

                            checks++;

                            if (checks > 1200)
                            {
                                checks = 0;
                                await sw.FlushAsync();

                                if (d.Day != DateTime.UtcNow.Day)
                                {
                                    await sw.DisposeAsync();
                                    fs.Close();

                                    d = DateTime.UtcNow;

                                    fs = new FileStream(
                                        Invariant($"Logs{Path.DirectorySeparatorChar}{d:yyyy-MM-ddTHH-mm-ss}.log"),
                                        FileMode.OpenOrCreate, FileAccess.Write,
                                        FileShare.ReadWrite);
                                    sw = new StreamWriter(fs)
                                    {
                                        AutoFlush = true
                                    };

                                    Log("Started logging.");
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(
                            Invariant(
                                $"[{DateTime.UtcNow:yyyy-MM-ddTHH:mm:ss.ff}] [Logging Exception!!!] {e.Message}"));
                    }
                }

            }
            finally
            {
                if (fs.CanRead)
                {
                    try
                    {
                        await sw.FlushAsync();
                        await sw.DisposeAsync();
                        fs.Close();
                    }
                    catch
                    {
                        //Ignore
                    }

                    try
                    {

                        fs.Close();
                    }
                    catch
                    {
                        //Ignore
                    }
                }
            }
        }

        private readonly struct LogEntry
        {
            public readonly string Message;
            public readonly LogEntryPriority Priority;
            public readonly DateTime Timestamp;
            public readonly LogType Type;

            public LogEntry(string message, LogEntryPriority priority, LogType type)
            {
                Message = message.Replace(Environment.NewLine, "\n", StringComparison.Ordinal);
                Priority = priority;
                Timestamp = DateTime.UtcNow;
                Type = type;
            }
        }

        public enum LogEntryPriority : byte
        {
            LiveView,
            CommandOutput,
            CommandInput,
            Info,
            Error,
            Critical
        }

        public enum LogType : byte
        {
            PrintAndLog,
            Print,
            Log
        }
    }
}