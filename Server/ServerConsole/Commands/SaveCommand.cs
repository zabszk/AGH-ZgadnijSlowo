namespace Server.ServerConsole.Commands
{
    [CommandHandler("save", "Saves all config files.")]
    public class SaveCommand : CommandBase
    {
        internal override void Execute(string[] args)
        {
            Logger.Log("Manual configs save has been started.", Logger.LogEntryPriority.CommandOutput);
            Program.Save();
        }
    }
}