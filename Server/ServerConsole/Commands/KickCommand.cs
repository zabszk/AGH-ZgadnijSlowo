using System;
using System.Linq;

namespace Server.ServerConsole.Commands
{
    [CommandHandler("kick", "Kicks a player.")]
    public class KickCommand : CommandBase
    {
        internal override void Execute(string[] args)
        {
            if (args.Length != 1)
            {
                Logger.Log("Syntax error: kick <username or remote endpoint>", Logger.LogEntryPriority.Error);
                return;
            }

            ServerClient c;
            
            lock (Program.Server.ClientsListLock)
            {
                c = Program.Server.Clients.FirstOrDefault(c =>
                    c.RemoteEndPoint.Equals(args[0], StringComparison.OrdinalIgnoreCase) || c.User != null &&
                    c.Username.Equals(args[0], StringComparison.OrdinalIgnoreCase));
            }

            if (c == null)
                Logger.Log("Requested player not found.", Logger.LogEntryPriority.Error);
            else
            {
                Logger.Log($"Player {c} has been disconnected.", Logger.LogEntryPriority.CommandOutput);
                c.Dispose();
            }
        }
    }
}