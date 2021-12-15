using System;
using System.Linq;
using System.Text;
using Server.Config;
using Server.Config.JsonObjects;

namespace Server.ServerConsole.Commands
{
    [CommandHandler("round", "Manages a round.")]
    public class RoundCommand : CommandBase
    {
        internal override void Execute(string[] args)
        {
            if (args.Length == 0)
            {
                Logger.Log(
                    "Subcommands:\nround list - lists users\nround add <short name> <full name> - creates a round\nround rm <short name> - removes a round\nround priority <short name> <new priority> - sets round display priority\nround rename <short name> <new full name> - renames a round\nround select <short name> - selects round as active",
                    Logger.LogEntryPriority.CommandOutput);
                return;
            }

            switch (args[0].ToLowerInvariant())
            {
                case "list":
                case "ls":
                    var sb = new StringBuilder();
                    sb.AppendLine("---- ROUNDS ----");
                    foreach (var u in ConfigManager.PrimaryConfig.Rounds)
                        sb.AppendFormat("{0}: {1} [{2}]{3}\n", u.ShortName, u.Name,
                            u.DisplayOrder < 0 ? "HIDDEN" : u.DisplayOrder,
                            u.ShortName.Equals(ConfigManager.PrimaryConfig.CurrentRound, StringComparison.Ordinal)
                                ? " >> CURRENT ROUND <<"
                                : "");

                    sb.Append("------------");

                    Logger.Log(sb.ToString(), Logger.LogEntryPriority.CommandOutput);
                    break;
                
                case "add" when args.Length < 3:
                    Logger.Log("Syntax error: round add <short name> <full name>", Logger.LogEntryPriority.Error);
                    break;
                
                case "add":
                {
                    string fullName = string.Join(' ', args.Skip(2));

                    foreach (var r in ConfigManager.PrimaryConfig.Rounds)
                    {
                        if (r.ShortName.Equals(args[1], StringComparison.OrdinalIgnoreCase))
                        {
                            Logger.Log("Round with this short name already exists.", Logger.LogEntryPriority.Error);
                            break;
                        }
                        
                        if (r.Name.Equals(fullName, StringComparison.OrdinalIgnoreCase))
                        {
                            Logger.Log("Round with this full name already exists.", Logger.LogEntryPriority.Error);
                            break;
                        }
                    }
                    
                    ConfigManager.PrimaryConfig.Rounds.Add(new Round(args[1].ToLowerInvariant(), fullName, 0));
                    ConfigManager.SavePrimary();

                    Logger.Log($"Round {args[1].ToLowerInvariant()} has been created.",
                        Logger.LogEntryPriority.CommandOutput);
                }
                    break;
                
                case "rm" when args.Length == 1:
                case "del" when args.Length == 1:
                    Logger.Log("Syntax error: round rm <short name>", Logger.LogEntryPriority.Error);
                    break;

                case "rm":
                case "del":
                    for (int i = 0; i < ConfigManager.PrimaryConfig.Rounds.Count; i++)
                        if (ConfigManager.PrimaryConfig.Rounds[i].ShortName
                            .Equals(args[1], StringComparison.OrdinalIgnoreCase))
                        {
                            ConfigManager.PrimaryConfig.Rounds.RemoveAt(i);
                            ConfigManager.SavePrimary();
                            Logger.Log("Specified round has been removed.", Logger.LogEntryPriority.Error);
                            break;
                        }
                    
                    Logger.Log("Specified round doesn't exist.", Logger.LogEntryPriority.Error);
                    break;
                
                case "priority" when args.Length == 3 && int.TryParse(args[2], out var priority):
                    for (int i = 0; i < ConfigManager.PrimaryConfig.Rounds.Count; i++)
                        if (ConfigManager.PrimaryConfig.Rounds[i].ShortName
                            .Equals(args[1], StringComparison.OrdinalIgnoreCase))
                        {
                            ConfigManager.PrimaryConfig.Rounds[i].SetPriority(priority);
                            ConfigManager.SavePrimary();
                            Logger.Log("Specified round has been updated.", Logger.LogEntryPriority.Error);
                            break;
                        }
                    
                    Logger.Log("Specified round doesn't exist.", Logger.LogEntryPriority.Error);
                    break;
                
                case "priority":
                    Logger.Log("Syntax error: round priority <short name> <new priority>", Logger.LogEntryPriority.Error);
                    break;
                
                case "rename" when args.Length < 3:
                case "mv" when args.Length < 3:
                    Logger.Log("Syntax error: round rename <short name> <new full name>", Logger.LogEntryPriority.Error);
                    break;
                
                case "rename":
                case "mv":
                    for (int i = 0; i < ConfigManager.PrimaryConfig.Rounds.Count; i++)
                        if (ConfigManager.PrimaryConfig.Rounds[i].ShortName
                            .Equals(args[1], StringComparison.OrdinalIgnoreCase))
                        {
                            ConfigManager.PrimaryConfig.Rounds[i].SetName(string.Join(' ', args.Skip(2)));
                            ConfigManager.SavePrimary();
                            Logger.Log("Specified round has been updated.", Logger.LogEntryPriority.Error);
                            break;
                        }
                    
                    Logger.Log("Specified round doesn't exist.", Logger.LogEntryPriority.Error);
                    break;
                
                case "select" when args.Length < 2:
                    Logger.Log("Syntax error: round select <short name>", Logger.LogEntryPriority.Error);
                    break;
                
                case "select":
                    var lw = args[1].ToLowerInvariant();
                    if (ConfigManager.PrimaryConfig.Rounds.All(r =>
                            !r.ShortName.Equals(lw, StringComparison.Ordinal)))
                    {
                        Logger.Log("Specified round doesn't exist.", Logger.LogEntryPriority.Error);
                        break;
                    }

                    ConfigManager.PrimaryConfig.CurrentRound = lw;
                    ConfigManager.SavePrimary();
                    Logger.Log("Current round has been updated.", Logger.LogEntryPriority.Error);
                    break;
                
                default:
                    Logger.Log("Syntax error", Logger.LogEntryPriority.Error);
                    break;
            }
        }
    }
}