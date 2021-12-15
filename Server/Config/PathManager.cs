using System.IO;

namespace Server.Config
{
    public static class PathManager
    {
        public static readonly string RoundLogsPath;

        static PathManager()
        {
            RoundLogsPath =
                $"{ConfigManager.PrimaryConfig.WebRootPath}{Path.DirectorySeparatorChar}GameLogs{Path.DirectorySeparatorChar}";
        }
    }
}