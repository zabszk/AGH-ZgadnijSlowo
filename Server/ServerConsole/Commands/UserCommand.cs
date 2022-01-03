using System;
using System.Collections.Generic;
using System.Text;
using Server.Config;
using Server.Config.JsonObjects;
using static System.FormattableString;

namespace Server.ServerConsole.Commands
{
    [CommandHandler("user", "Manages a user.")]
    [CommandAlias("u")]
    [ConfidentialCommand]
    public class UserCommand : CommandBase
    {
        internal override void Execute(string[] args)
        {
            if (args.Length == 0)
            {
                Logger.Log("Subcommands:\nuser list - lists users\nuser add <username> [password] - creates a user\nuser rm <username> - removes a user\nuser passwd <username> [new password] - changes password\nuser suspend <username>", Logger.LogEntryPriority.CommandOutput);
                return;
            }

            switch (args[0].ToLowerInvariant())
            {
                case "list":
                case "ls":
                    var sb = new StringBuilder();
                    sb.AppendLine("---- USERS ----");
                    foreach (var u in ConfigManager.Users)
                    {
                        sb.AppendFormat("- {0}{1}\nLast login: {2}\nScore in rounds:", u.Key,
                            u.Value.Suspended ? " !! SUSPENDED !!" : "",
                            u.Value.LastLogin.Ticks == 0
                                ? "never"
                                : Invariant($"{u.Value.LastLogin:yyyy-MM-dd HH:mm:ss.ff}"));
                        if (u.Value.Score.Count == 0)
                            sb.AppendLine(" (none)\n");
                        else
                        {
                            sb.AppendLine();

                            foreach (var round in u.Value.Score)
                                sb.AppendFormat("- {0}: {1}\n", round.Key, round.Value);

                            sb.AppendLine();
                        }
                    }

                    sb.Append("------------");

                    Logger.Log(sb.ToString(), Logger.LogEntryPriority.CommandOutput);
                    break;

                case "add" when args.Length == 1:
                    Logger.Log("Syntax error: user add <username> [password]", Logger.LogEntryPriority.Error);
                    break;

                case "add":
                {
                    if (ConfigManager.Users.ContainsKey(args[1].ToLowerInvariant()))
                    {
                        Logger.Log("User this username already exists.", Logger.LogEntryPriority.Error);
                        break;
                    }

                    var password = args.Length > 2 ? args[2] : Misc.SecureRandomGenerator.GeneratePassword();
                    ConfigManager.Users.Add(args[1].ToLowerInvariant(), new User(
                        Misc.Sha.HashToString(Misc.Sha.Sha512(password)), new Dictionary<string, int>(),
                        false, new DateTime(0)));
                    ConfigManager.SaveUsers();

                    Logger.Log($"User {args[1].ToLowerInvariant()} has been created.",
                        Logger.LogEntryPriority.CommandOutput);
                    if (args.Length <= 2)
                        Logger.Log($"Password: {password}", Logger.LogEntryPriority.CommandOutput,
                            type: Logger.LogType.Print);
                }
                    break;

                case "rm" when args.Length == 1:
                case "del" when args.Length == 1:
                    Logger.Log("Syntax error: user rm <username>", Logger.LogEntryPriority.Error);
                    break;

                case "rm":
                case "del":
                    if (!ConfigManager.Users.ContainsKey(args[1].ToLowerInvariant()))
                    {
                        Logger.Log("Specified user doesn't exist.", Logger.LogEntryPriority.Error);
                        break;
                    }

                    var score = ConfigManager.Users[args[1].ToLowerInvariant()].Score;
                    var ll = ConfigManager.Users[args[1].ToLowerInvariant()].LastLogin;
                    ConfigManager.Users.Remove(args[1].ToLowerInvariant());
                    ConfigManager.SaveUsers();
                    
                    Logger.Log(Invariant($"User {args[1].ToLowerInvariant()} has been removed.\nDetails of the removed user:\nLast login: {ll:yyyy-MM-dd HH:mm:ss.ff}"), Logger.LogEntryPriority.CommandOutput);
                    break;
                
                case "passwd" when args.Length == 1:
                    Logger.Log("Syntax error: user passwd <username> [new password]", Logger.LogEntryPriority.Error);
                    break;

                case "passwd":
                {
                    if (!ConfigManager.Users.ContainsKey(args[1].ToLowerInvariant()))
                    {
                        Logger.Log("Specified user doesn't exist.", Logger.LogEntryPriority.Error);
                        break;
                    }

                    var password = args.Length > 2 ? args[2] : Misc.SecureRandomGenerator.GeneratePassword();
                    ConfigManager.Users[args[1]].Password = Misc.Sha.HashToString(Misc.Sha.Sha512(password));
                    ConfigManager.SaveUsers();

                    if (args.Length > 2)
                        Logger.Log($"Password of user {args[1].ToLowerInvariant()} has ben changed.", Logger.LogEntryPriority.CommandOutput);
                    else
                    {
                        Logger.Log($"Password of user {args[1].ToLowerInvariant()} has ben changed.", Logger.LogEntryPriority.CommandOutput, type: Logger.LogType.Log);
                        Logger.Log($"Password of user {args[1].ToLowerInvariant()} has ben changed to {password}.", Logger.LogEntryPriority.CommandOutput, type: Logger.LogType.Print);
                    }
                }
                    break;
                
                case "suspend" when args.Length == 1:
                    Logger.Log("Syntax error: user suspend <username>", Logger.LogEntryPriority.Error);
                    break;
                
                case "suspend":
                    if (!ConfigManager.Users.ContainsKey(args[1].ToLowerInvariant()))
                    {
                        Logger.Log("Specified user doesn't exist.", Logger.LogEntryPriority.Error);
                        break;
                    }

                    ConfigManager.Users[args[1]].Suspended = !ConfigManager.Users[args[1]].Suspended;
                    ConfigManager.SaveUsers();
                    
                    if (ConfigManager.Users[args[1]].Suspended)
                    {
                        Logger.Log($"User {args[1]} has been suspended.", Logger.LogEntryPriority.CommandOutput);
                        
                        for (int i = Program.Server.Clients.Count - 1; i >= 0; i--)
                            if (Program.Server.Clients[i].Username.Equals(args[1], StringComparison.OrdinalIgnoreCase))
                                Program.Server.Clients[i].Dispose();
                    }
                    else Logger.Log($"User {args[1]} is no longer suspended.", Logger.LogEntryPriority.CommandOutput);
                    break;
                
                default:
                    Logger.Log("Syntax error", Logger.LogEntryPriority.Error);
                    break;
            }
        }
    }
}