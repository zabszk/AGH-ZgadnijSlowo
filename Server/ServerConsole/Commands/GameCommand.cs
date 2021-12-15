using System.Linq;

namespace Server.ServerConsole.Commands
{
    [CommandHandler("game", "Manages a game.")]
    [CommandAlias("g")]
    public class GameCommand : CommandBase
    {
        internal override void Execute(string[] args)
        {
            if (args.Length != 2 || !uint.TryParse(args[1], out var id))
            {
                Logger.Log("Syntax error: game <start/terminate> <internal game ID>", Logger.LogEntryPriority.Error);
                return;
            }

            lock (Program.Server.GameQueueLock)
            {
                var game = Program.Server.GameQueue.FirstOrDefault(g => g.InternalId == id);

                if (game == null)
                {
                    Logger.Log("Game with provided Internal ID doesn't exist.", Logger.LogEntryPriority.Error);
                    return;
                }

                switch (args[0].ToLowerInvariant())
                {
                    case "start" when game.Players.Count >= 2:
                    case "s" when game.Players.Count >= 2:
                        game.InProgress = true;
                        Logger.Log("Game has been started.", Logger.LogEntryPriority.CommandOutput);
                        break;

                    case "start":
                    case "s":
                        Logger.Log("There must be at least 2 players in the game to start the game.",
                            Logger.LogEntryPriority.Error);
                        break;

                    case "terminate":
                    case "t":
                        game.Terminate();
                        Logger.Log("Game has been terminated.", Logger.LogEntryPriority.CommandOutput);
                        break;

                    default:
                        Logger.Log("Syntax error: game <start/terminate> <internal game ID>",
                            Logger.LogEntryPriority.Error);
                        break;
                }
            }
        }
    }
}