using System.Text;

namespace Server.ServerConsole.Commands
{
    [CommandHandler("help", "Prints this help.")]
    public class HelpCommand : CommandBase
    {
        internal override void Execute(string[] args)
        {
            var sb = new StringBuilder();
            sb.AppendLine("---- HELP ----");
            foreach (var cmd in InputHandler.Commands.Values)
            {
                sb.AppendFormat("- {0} - {1}\n", cmd.Name, cmd.Description);
                if (cmd.Aliases.Count > 0)
                    sb.AppendFormat("Aliases: {0}\n\n", string.Join(", ", cmd.Aliases));
                else sb.AppendLine();
            }
            sb.Append("------------");
            
            Logger.Log(sb.ToString(), Logger.LogEntryPriority.CommandOutput);
        }
    }
}