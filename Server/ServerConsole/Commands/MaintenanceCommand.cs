﻿namespace Server.ServerConsole.Commands
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
                    $"Current maintenance mode status:\nBlock new connections: {NoIncomingConnections}\nAdministrative hold: {NoAutoStart}\n\nModifying modes: maintenance <mode to toggle>",
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
                
                default:
                    Logger.Log(
                        "Invalid mode name.\nValid modes:\n- connections / cn / conn / tcp / block - Blocking new connections\n- hold / adm / autostart / auto / start / games / timer - Administrative hold",
                        Logger.LogEntryPriority.Error);
                    break;
            }
        }
    }
}