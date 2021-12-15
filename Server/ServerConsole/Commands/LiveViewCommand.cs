namespace Server.ServerConsole.Commands
{
    [CommandHandler("liveview", "Toggles the live view.")]
    [CommandAlias("lv")]
    public class LiveViewCommand : CommandBase
    {
        internal override void Execute(string[] args)
        {
            Logger.LiveView = !Logger.LiveView;
            Logger.Log($"Live view has been {(Logger.LiveView ? "enabled" : "disabled")}.", Logger.LogEntryPriority.CommandOutput);
        }
    }
}