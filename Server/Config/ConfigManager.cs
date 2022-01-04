using System;
using System.Collections.Generic;
using System.IO;
using Server.Config.JsonObjects;
using Server.Config.JsonObjects.OldJsonObjects;
using Server.ServerConsole;
using Utf8Json;
using Utf8Json.Resolvers;
using User = Server.Config.JsonObjects.User;

namespace Server.Config
{
    public static class ConfigManager
    {
        public static PrimaryConfig PrimaryConfig;
        public static UsersConfig UsersConfig;
        public static ServerWordsStorage WordsStorage;

        private const int CurrentUsersConfigVersion = 2;

        private static readonly object PrimaryLock = new(), UsersLock = new(), ScoreboardLock = new();

        public static Dictionary<string, User> Users => UsersConfig.Users;

        static ConfigManager()
        {
            CompositeResolver.RegisterAndSetAsDefault(GeneratedResolver.Instance,
                BuiltinResolver.Instance);
        }
        
        public static bool Load()
        {
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
                    PrimaryConfig = new PrimaryConfig("0.0.0.0", 7777, 1, 10, 300,
                        new List<Round> { new("def", "Default round", 0) }, "def", 0, "webroot", true, true, CurrentUsersConfigVersion);
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
                Logger.VerboseView = PrimaryConfig.VerboseView;
                Logger.Log("Primary config loaded.");
            }
            
            // ReSharper disable once InconsistentlySynchronizedField
            if (PrimaryConfig.MinimumPlayersAmount < 1)
            {
                // ReSharper disable once InconsistentlySynchronizedField
                PrimaryConfig.MinimumPlayersAmount = 1;
                SavePrimary();
            }
            
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
                    
                    if (PrimaryConfig.UsersFileVersion != CurrentUsersConfigVersion)
                    {
                        PrimaryConfig.UsersFileVersion = CurrentUsersConfigVersion;
                        SavePrimary();
                    }
                }
                else
                {
                    if (PrimaryConfig.UsersFileVersion < 2)
                    {
                        MigrateV1UsersConfig();
                        
                        PrimaryConfig.UsersFileVersion = CurrentUsersConfigVersion;
                        SavePrimary();
                    }
                    else
                    {
                        var fs = new FileStream("users.json", FileMode.Open, FileAccess.Read,
                            FileShare.ReadWrite);

                        UsersConfig = JsonSerializer.Deserialize<UsersConfig>(fs);
                        fs.Close();
                    }
                }

                Logger.Log("Users config loaded.");
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

        private static void MigrateV1UsersConfig()
        {
            Logger.Log("Migrating V1 users config...");
            
            var fs = new FileStream("users.json", FileMode.Open, FileAccess.Read,
                FileShare.ReadWrite);

            var old = JsonSerializer.Deserialize<OldUsersConfig>(fs);
            fs.Close();
            
            UsersConfig = new UsersConfig(new Dictionary<string, User>(old.Users.Count));
            foreach (var user in old.Users)
                UsersConfig.Users.Add(user.Key, new User(user.Value));
            
            File.Move("users.json", "users-v1.json");
            
            fs = new FileStream("users.json", FileMode.CreateNew, FileAccess.Write,
                FileShare.ReadWrite);
            JsonSerializer.Serialize(fs, UsersConfig);
            fs.Close();
            
            Logger.Log("Migrated users config from V1 to V2.");
        }

        public static void GenerateScoreboard()
        {
            try
            {
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
                        new CurrentConfig(PrimaryConfig.CurrentRound, PrimaryConfig.PlayersLimit,
                            PrimaryConfig.MinimumPlayersAmount, PrimaryConfig.GameDelay),
                        su, g);
                }

                lock (ScoreboardLock)
                {
                    var fs = new FileStream(PathManager.ScoreboardPath, FileMode.Create, FileAccess.Write,
                        FileShare.ReadWrite);
                    JsonSerializer.Serialize(fs, sb);
                    fs.Close();
                }

                Logger.Log("Scoreboard has been generated.", Logger.LogEntryPriority.LiveView, uint.MaxValue,
                    Logger.LogType.Print);
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to generate scoreboard: {e.Message}", Logger.LogEntryPriority.Error);
            }
        }
    }
}