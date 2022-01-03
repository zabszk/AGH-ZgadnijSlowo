using Server.Config;

namespace Server.ServerConsole.Commands
{
    [CommandHandler("gamerule", "Modifies game rules.")]
    [CommandAlias("gr")]
    public class GameRuleCommand : CommandBase
    {
        internal override void Execute(string[] args)
        {
            if (args.Length != 2)
            {
                Logger.Log(
                    $"Current game rules:\nMinimum amount of players: {ConfigManager.PrimaryConfig.MinimumPlayersAmount}\nPlayers limit: {ConfigManager.PrimaryConfig.PlayersLimit}\nGame delay: {ConfigManager.PrimaryConfig.GameDelay}\n\nModifying game rules: gamerule <rule name> <new value>",
                    Logger.LogEntryPriority.CommandOutput);
                return;
            }

            switch (args[0].ToLowerInvariant())
            {
                case "playerslimit":
                case "pl":
                case "limit":
                {
                    if (!ushort.TryParse(args[1], out var limit))
                    {
                        Logger.Log("New limit must be an unsigned short integer.", Logger.LogEntryPriority.Error);
                        return;
                    }

                    ConfigManager.PrimaryConfig.PlayersLimit = limit;
                    ConfigManager.SavePrimary();
                    Logger.Log("Game rule has been updated.", Logger.LogEntryPriority.CommandOutput);
                }
                    break;
                
                case "minimum":
                case "min":
                case "amount":
                {
                    if (!ushort.TryParse(args[1], out var min) || min < 1)
                    {
                        Logger.Log("New minimum amount must be a positive short integer.", Logger.LogEntryPriority.Error);
                        return;
                    }

                    ConfigManager.PrimaryConfig.MinimumPlayersAmount = min;
                    ConfigManager.SavePrimary();
                    Logger.Log("Game rule has been updated.", Logger.LogEntryPriority.CommandOutput);
                }
                    break;

                case "gamedelay":
                case "gd":
                case "delay":
                {
                    if (!ushort.TryParse(args[1], out var delay))
                    {
                        Logger.Log("New game delay must be an unsigned short integer.", Logger.LogEntryPriority.Error);
                        return;
                    }
                    
                    ConfigManager.PrimaryConfig.GameDelay = delay;
                    ConfigManager.SavePrimary();
                    Logger.Log("Game rule has been updated.", Logger.LogEntryPriority.CommandOutput);
                }
                    break;
                
                default:
                    Logger.Log(
                        "Invalid game rule.\nValid rules:\n- minimum / min / amount - Minimum amount of players\n- playerslimit / pl / limit - Players limit\n- gamedelay / gd / delay - Game delay",
                        Logger.LogEntryPriority.Error);
                    break;
            }
        }
    }
}