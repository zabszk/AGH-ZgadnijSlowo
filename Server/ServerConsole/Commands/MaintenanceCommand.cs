namespace Server.ServerConsole.Commands
{
    [CommandHandler("maintenance", "Manages maintenance mode.")]
    [CommandAlias("mn")]
    [CommandAlias("mnt")]
    public class MaintenanceCommand : CommandBase
    {
        public static bool NoIncomingConnections, NoAutoStart;
        
        internal override void Execute(string[] args)
        {
            if (args.Length != 1)
            {
                Logger.Log(
                    $"Current maintenance mode status:\nBlock new connections: {NoIncomingConnections}\nDisable games autostart: {NoAutoStart}\n\nModifying modes: maintenance <mode to toggle>",
                    Logger.LogEntryPriority.CommandOutput);
                return;
            }

            switch (args[0].ToLowerInvariant())
            {
                case "connections":
                case "cn":
                case "conn":
                case "tcp":
                case "block":
                    NoIncomingConnections = !NoIncomingConnections;
                    Logger.Log($"Blocking incoming connections has been {(NoIncomingConnections ? "enabled" : "disabled")}.", Logger.LogEntryPriority.CommandOutput);
                    break;
                
                case "autostart":
                case "auto":
                case "start":
                case "games":
                case "timer":
                case "hold":
                    NoAutoStart = !NoAutoStart;

                    lock (Program.Server.GameQueueLock)
                    {
                        Program.Server.GameQueue.ForEach(g => g.PlayersChanged(true));
                    }
                    
                    Logger.Log($"Preventing games from autostarting has been {(NoAutoStart ? "enabled" : "disabled")}.", Logger.LogEntryPriority.CommandOutput);
                    break;
                
                default:
                    Logger.Log(
                        "Invalid mode name.\nValid modes:\n- connections / cn / conn / tcp / block - Blocking new connections\n- autostart / auto / start / games / timer / hold - Disabling games autostart",
                        Logger.LogEntryPriority.Error);
                    break;
            }
        }
    }
}