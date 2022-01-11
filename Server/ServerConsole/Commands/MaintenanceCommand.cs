namespace Server.ServerConsole.Commands
{
    [CommandHandler("maintenance", "Manages maintenance mode.")]
    [CommandAlias("mn")]
    [CommandAlias("mnt")]
    public class MaintenanceCommand : CommandBase
    {
        public static bool NoIncomingConnections, NoAutoStart;
        private static uint _autoEnableNoAutostart;

        private static readonly object AutoEnableLock = new();
        
        internal override void Execute(string[] args)
        {
            if (args.Length == 0)
            {
                Logger.Log(
                    $"Current maintenance mode status:\nBlock new connections: {NoIncomingConnections}\nAdministrative hold: {NoAutoStart}\nAutoenable administrative hold: {(_autoEnableNoAutostart == 0 ? "disabled" : $"after {_autoEnableNoAutostart} {(_autoEnableNoAutostart == 1 ? "round" : "rounds")}")}\n\nModifying modes: maintenance <mode to toggle>",
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
                
                case "hold":
                case "adm":
                case "autostart":
                case "auto":
                case "start":
                case "games":
                case "timer":
                    NoAutoStart = !NoAutoStart;

                    lock (Program.Server.GameQueueLock)
                    {
                        Program.Server.GameQueue.ForEach(g => g.PlayersChanged(true));
                    }
                    
                    Logger.Log($"Administrative hold of games has been {(NoAutoStart ? "enabled" : "disabled")}.", Logger.LogEntryPriority.CommandOutput);
                    break;
                    
                case "autohold" when args.Length == 2 && uint.TryParse(args[1], out uint ah):
                    lock (AutoEnableLock)
                    {
                        _autoEnableNoAutostart = ah;
                    }
                    
                    Logger.Log(
                        ah == 0
                            ? "Auto administrative hold has been disabled."
                            : $"Auto administrative hold has been set to {ah} rounds.",
                        Logger.LogEntryPriority.CommandOutput);
                    break;
                
                default:
                    Logger.Log(
                        "Invalid mode name.\nValid modes:\n- connections / cn / conn / tcp / block - Blocking new connections\n- hold / adm / autostart / auto / start / games / timer - Administrative hold\n- autohold <amount of rounds or 0 to disable> - Administrative hold autoenabling",
                        Logger.LogEntryPriority.Error);
                    break;
            }
        }

        internal static bool HandleAutoEnable()
        {
            lock (AutoEnableLock)
            {
                if (_autoEnableNoAutostart == 0)
                    return !NoAutoStart;
                
                _autoEnableNoAutostart--;
                
                if (_autoEnableNoAutostart == 0)
                    NoAutoStart = true;

                return true;
            }
        }
    }
}