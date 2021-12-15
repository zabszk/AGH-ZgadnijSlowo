using System.Text;

namespace Server.ServerConsole.Commands
{
    [CommandHandler("clients", "Lists connected clients.")]
    [CommandAlias("list")]
    [CommandAlias("players")]
    public class ClientsCommand : CommandBase
    {
        internal override void Execute(string[] args)
        {
            var sb = new StringBuilder();
            sb.AppendLine("---- CLIENTS ----");
            lock (Program.Server.ClientsListLock)
            {
                foreach (var client in Program.Server.Clients)
                    sb.AppendLine(client.ToString());
            }
            sb.Append("------------");
            
            Logger.Log(sb.ToString(), Logger.LogEntryPriority.CommandOutput);
        }
    }
}