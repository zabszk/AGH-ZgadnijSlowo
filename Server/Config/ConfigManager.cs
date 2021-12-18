using System;
using System.Collections.Generic;
using System.IO;
using Server.Config.JsonObjects;
using Server.ServerConsole;
using Utf8Json;
using Utf8Json.Resolvers;

namespace Server.Config
{
    public static class ConfigManager
    {
        public static PrimaryConfig PrimaryConfig;
        public static UsersConfig UsersConfig;
        public static ServerWordsStorage WordsStorage;

        private static readonly object PrimaryLock = new(), UsersLock = new(), ScoreboardLock = new();

        public static Dictionary<string, User> Users => UsersConfig.Users;

        static ConfigManager()
        {
            CompositeResolver.RegisterAndSetAsDefault(GeneratedResolver.Instance,
                BuiltinResolver.Instance);
        }
        
        public static bool Load()
        {
            lock (UsersLock)
            {
                if (File.Exists("users.json.old"))
                {
                    if (File.Exists("users.json"))
                        File.Delete("users.json");
                    
                    File.Move("users.json.old", "users.json");
                }
                
                if (!File.Exists("users.json"))
                {
                    UsersConfig = new UsersConfig(new Dictionary<string, User>());
                    var fs = new FileStream("users.json", FileMode.CreateNew, FileAccess.Write,
                        FileShare.ReadWrite);
                    JsonSerializer.Serialize(fs, UsersConfig);
                    fs.Close();
                }
                else
                {
                    var fs = new FileStream("users.json", FileMode.Open, FileAccess.Read,
                        FileShare.ReadWrite);

                    UsersConfig = JsonSerializer.Deserialize<UsersConfig>(fs);
                    fs.Close();
                }

                Logger.Log("Users config loaded.");
            }

            lock (PrimaryLock)
            {
                if (File.Exists("config.json.old"))
                {
                    if (File.Exists("config.json"))
                        File.Delete("config.json");
                    
                    File.Move("config.json.old", "config.json");
                }
                
                if (!File.Exists("config.json"))
                {
                    PrimaryConfig = new PrimaryConfig("0.0.0.0", 7777, 10, 300,
                        new List<Round> { new("def", "Default round", 0) }, "def", 0, "webroot", true);
                    var fs = new FileStream("config.json", FileMode.CreateNew, FileAccess.Write,
                        FileShare.ReadWrite);
                    JsonSerializer.Serialize(fs, PrimaryConfig);
                    fs.Close();
                    return false;
                }

                {
                    var fs = new FileStream("config.json", FileMode.Open, FileAccess.Read,
                        FileShare.ReadWrite);

                    PrimaryConfig = JsonSerializer.Deserialize<PrimaryConfig>(fs);
                    fs.Close();
                }

                Logger.LiveView = PrimaryConfig.LiveView;
                Logger.Log("Primary config loaded.");
            }

            WordsStorage = new ServerWordsStorage();
            if (WordsStorage.Words.Count != 0)
                Logger.Log("Words database loaded.");
            
            return true;
        }

        public static void SavePrimary()
        {
            lock (PrimaryLock)
            {
                try
                {
                    File.Move("config.json", "config.json.old");
                    
                    var fs = new FileStream("config.json", FileMode.Create, FileAccess.Write,
                        FileShare.ReadWrite);
                    JsonSerializer.Serialize(fs, PrimaryConfig);
                    fs.Close();
                    
                    File.Delete("config.json.old");
                }
                catch (Exception e)
                {
                    Logger.Log($"Saving primary config failed: {e.Message}", Logger.LogEntryPriority.Critical);
                }
            }
        }
        
        public static void SaveUsers()
        {
            lock (UsersLock)
            {
                try
                {
                    File.Move("users.json", "users.json.old");
                    
                    var fs = new FileStream("users.json", FileMode.Create, FileAccess.Write,
                        FileShare.ReadWrite);
                    JsonSerializer.Serialize(fs, UsersConfig);
                    fs.Close();
                    
                    File.Delete("users.json.old");
                }
                catch (Exception e)
                {
                    Logger.Log($"Saving users config failed: {e.Message}", Logger.LogEntryPriority.Critical);
                }
            }
        }

        public static void GenerateScoreboard()
        {
            Logger.Log("Generating scoreboard...", Logger.LogEntryPriority.LiveView, uint.MaxValue, Logger.LogType.Print);
            Scoreboard sb;
            List<ScoreboardGame> g;
            List<ScoreboardUser> su = new(Users.Count);

            foreach (var u in Users)
            {
                if (u.Value.Suspended)
                    continue;

                su.Add(new ScoreboardUser(u.Key, u.Value.Score));
            }

            lock (Program.Server.GameQueueLock)
            {
                g = new(Program.Server.GameQueue.Count);
                foreach (var game in Program.Server.GameQueue)
                    g.Add(new(game));
            }

            lock (PrimaryLock)
            {
                sb = new Scoreboard(DateTime.UtcNow, Core.Version.VersionString, PrimaryConfig.Rounds,
                    new CurrentConfig(PrimaryConfig.CurrentRound, PrimaryConfig.PlayersLimit, PrimaryConfig.GameDelay),
                    su, g);
            }

            lock (ScoreboardLock)
            {
                var fs = new FileStream(PathManager.ScoreboardPath, FileMode.Create, FileAccess.Write,
                        FileShare.ReadWrite);
                JsonSerializer.Serialize(fs, sb);
                fs.Close();
            }
            
            Logger.Log("Scoreboard has been generated.", Logger.LogEntryPriority.LiveView, uint.MaxValue, Logger.LogType.Print);
        }
    }
}