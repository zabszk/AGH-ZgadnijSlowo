namespace Server.ServerConsole.Commands
{
    [CommandHandler("exit", "Shutdowns the server.")]
    [CommandAlias("quit")]
    [CommandAlias("shutdown")]
    public class ExitCommand : CommandBase
    {
        internal override void Execute(string[] args)
        {
            Logger.Log("Shutting down (command)...", Logger.LogEntryPriority.CommandOutput);
            Program.Exit();
        }
    }
}