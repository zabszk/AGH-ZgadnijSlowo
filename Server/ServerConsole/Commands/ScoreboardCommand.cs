using Server.Config;

namespace Server.ServerConsole.Commands
{
    [CommandHandler("scoreboard", "Generates the scoreboard.")]
    [CommandAlias("sb")]
    [CommandAlias("gen")]
    public class ScoreboardCommand : CommandBase
    {
        internal override void Execute(string[] args)
        {
            ConfigManager.GenerateScoreboard();
        }
    }
}