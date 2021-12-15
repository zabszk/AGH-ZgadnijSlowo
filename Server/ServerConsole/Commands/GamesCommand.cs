using System.Text;

namespace Server.ServerConsole.Commands
{
    [CommandHandler("games", "Lists connected games.")]
    [CommandAlias("gs")]
    public class GamesCommand : CommandBase
    {
        internal override void Execute(string[] args)
        {
            var sb = new StringBuilder();
            sb.AppendLine("---- GAMES ----");
            lock (Program.Server.GameQueueLock)
            {
                foreach (var game in Program.Server.GameQueue)
                    sb.AppendLine(game.ToString());
            }
            sb.Append("------------");
            
            Logger.Log(sb.ToString(), Logger.LogEntryPriority.CommandOutput);
        }
    }
}