namespace Server.ServerConsole.Commands
{
    [CommandHandler("verboseview", "Toggles the verbose view.")]
    [CommandAlias("verbose")]
    [CommandAlias("vb")]
    public class VerboseViewCommand : CommandBase
    {
        internal override void Execute(string[] args)
        {
            Logger.VerboseView = !Logger.VerboseView;
            Logger.Log($"Verbose view has been {(Logger.VerboseView ? "enabled" : "disabled")}.", Logger.LogEntryPriority.CommandOutput);
        }
    }
}