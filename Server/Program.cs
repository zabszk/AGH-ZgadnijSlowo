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
        internal static readonly CancellationTokenSource Cts = new (), LoggerCts = new();
        public static readonly UTF8Encoding Encoder = new ();
        internal static Server Server;
        private static uint _cancelling;

        private static void Main(string[] args)
        {
            Console.CancelKeyPress += (sender, eventArgs) =>
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

            Console.WriteLine($"ZgadnijSlowo Server, v. {Core.Version.VersionString}");
            Console.WriteLine("Copyright by Łukasz Jurczyk, 2021");
            Console.WriteLine("Licensed under the MIT License.");
            Console.WriteLine("Loading configs...");
            if (!ConfigManager.Load())
            {
                Console.WriteLine("Config files have been generated!");
                return;
            }
            Console.WriteLine("All configs loaded.");

            if (!IPAddress.TryParse(ConfigManager.PrimaryConfig.ListeningIp, out var ip))
            {
                Console.WriteLine("Failed to parse Listening IP!");
                return;
            }

            if (ConfigManager.PrimaryConfig.CurrentRound == null || ConfigManager.PrimaryConfig.Rounds.All(r =>
                !r.ShortName.Equals(ConfigManager.PrimaryConfig.CurrentRound, StringComparison.Ordinal)))
            {
                Console.WriteLine("Invalid Current Round - null or doesn't exist.");
                return;
            }

            InputHandler.Init();
            Server = new Server(new IPEndPoint(ip, ConfigManager.PrimaryConfig.ListeningPort));
            var loggerTask = Logger.QueueTask(LoggerCts.Token);
            InputCapture.Start(Cts.Token);
            
            Task.WaitAll(Server.Start(Cts.Token), Server.TimeoutClients(Cts.Token), AutoSave(Cts.Token));
            
            Server.Dispose();
            Save();
            LoggerCts.Cancel();
            loggerTask.Wait();
        }

        internal static void Exit()
        {
            _cancelling = 1;
            Cts.Cancel();
            Server.Dispose();
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