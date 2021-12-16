using System.IO;
using Server.ServerConsole;

namespace Server.Config
{
    public static class PathManager
    {
        public static readonly string RoundLogsPath, ScoreboardPath;

        static PathManager()
        {
            RoundLogsPath =
                $"{ConfigManager.PrimaryConfig.WebRootPath}{Path.DirectorySeparatorChar}GameLogs{Path.DirectorySeparatorChar}";
            
            ScoreboardPath =
                $"{ConfigManager.PrimaryConfig.WebRootPath}{Path.DirectorySeparatorChar}scoreboard.json";

            if (!Directory.Exists(ConfigManager.PrimaryConfig.WebRootPath))
            {
                Logger.Log("Creating webroot directory...");
                Directory.CreateDirectory(ConfigManager.PrimaryConfig.WebRootPath);
            }
            
            if (!Directory.Exists(RoundLogsPath))
            {
                Logger.Log("Creating Game Logs directory...");
                Directory.CreateDirectory(RoundLogsPath);
            }
        }
    }
}