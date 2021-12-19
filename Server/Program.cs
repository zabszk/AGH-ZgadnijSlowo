using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Server.Config;
using Server.ServerConsole;

namespace Server
{
    internal static class Program
    {
        private static readonly CancellationTokenSource Cts = new(), LoggerCts = new();
        public static readonly UTF8Encoding Encoder = new ();
        internal static Server Server;
        private static uint _cancelling;

        private static void Main()
        {
            Console.CancelKeyPress += (_, eventArgs) =>
            {
                switch (_cancelling)
                {
                    case 0:
                        eventArgs.Cancel = true;
                        Logger.Log("Shutting down (CTRL + C)...", Logger.LogEntryPriority.CommandOutput);
                        Exit();
                        break;
                    
                    case 1:
                        eventArgs.Cancel = true;
                        _cancelling = 2;
                        Logger.Log("Server is already shutting down. Press CTRL + C one more time to force shutdown.", Logger.LogEntryPriority.Error);
                        break;
                    
                    default:
                        Logger.Log("Forcing shutdown...", Logger.LogEntryPriority.CommandOutput);
                        return;
                }
            };

            Task loggerTask = null;
            
            try
            {
                Logger.Log($"ZgadnijSlowo Server, v.{Core.Version.VersionString}");
                Logger.Log("Copyright by Łukasz Jurczyk, 2021");
                Logger.Log("Licensed under the MIT License.");
                Logger.Log("Loading configs...");
                loggerTask = Logger.QueueTask(LoggerCts.Token);
                
                if (!ConfigManager.Load())
                {
                    Logger.Log("Config files have been generated!");
                    return;
                }

                if (ConfigManager.WordsStorage.Words.Count == 0)
                    return;

                Logger.Log("All configs loaded.");

                if (!IPAddress.TryParse(ConfigManager.PrimaryConfig.ListeningIp, out var ip))
                {
                    Logger.Log("Failed to parse Listening IP!", Logger.LogEntryPriority.Critical);
                    return;
                }

                if (ConfigManager.PrimaryConfig.CurrentRound == null || ConfigManager.PrimaryConfig.Rounds.All(r =>
                        !r.ShortName.Equals(ConfigManager.PrimaryConfig.CurrentRound, StringComparison.Ordinal)))
                {
                    Logger.Log("Invalid Current Round - null or doesn't exist.", Logger.LogEntryPriority.Critical);
                    return;
                }

                InputHandler.Init();
                Server = new Server(new IPEndPoint(ip, ConfigManager.PrimaryConfig.ListeningPort));
                InputCapture.Start(Cts.Token);

                Task.WaitAll(Server.Start(Cts.Token), Server.TimeoutClients(Cts.Token), AutoSave(Cts.Token),
                    ScoreboardGenerationTask(Cts.Token));
            }
            finally
            {
                Server?.Dispose();
                Save();
                LoggerCts.Cancel();
                loggerTask?.Wait();
            }
        }

        internal static void Exit()
        {
            _cancelling = 1;
            Cts.Cancel();
            Server?.Dispose();
        }

        private static async Task AutoSave(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(120000, token);
                }
                catch (TaskCanceledException)
                {
                    //Ignore
                }

                if (token.IsCancellationRequested)
                    break;
                
                Logger.Log("Running autosave...");
                Save();
                Logger.Log("Autosave completed.");
            }
        }

        private static async Task ScoreboardGenerationTask(CancellationToken token)
        {
            ConfigManager.GenerateScoreboard();

            while (!token.IsCancellationRequested)
            {
                try
                {
                    try
                    {
                        await Task.Delay(6000, token);
                    }
                    catch (TaskCanceledException)
                    {
                        //Ignore
                    }

                    ConfigManager.GenerateScoreboard();
                }
                catch (Exception e)
                {
                    Logger.Log($"Failed to generate the scoreboard: {e.Message}", Logger.LogEntryPriority.Error);
                }
            }
        }

        internal static void Save()
        {
            Logger.Log("Saving primary config...");
            ConfigManager.SavePrimary();
            Logger.Log("Saving users config...");
            ConfigManager.SaveUsers();
            Logger.Log("Configs saved.");
        }
    }
}