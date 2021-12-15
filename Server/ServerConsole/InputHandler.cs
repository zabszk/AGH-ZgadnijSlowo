using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Server.ServerConsole
{
    internal static class InputHandler
    {
        internal static readonly Dictionary<string, CommandBase> Commands = new(StringComparer.OrdinalIgnoreCase);
        private static readonly Dictionary<string, CommandBase> Aliases = new(StringComparer.OrdinalIgnoreCase);

        internal static void Init()
        {
            RegisterCommands();
            InputCapture.InputCapturedEvent += HandleInput;
        }

        private static void RegisterCommand(CommandBase command)
        {
            Commands.Add(command.Name, command);
            foreach (string s in command.Aliases)
                Aliases.Add(s, command);
        }

        private static CommandBase GetCommandByName(string name) => Commands.TryGetValue(name, out var command) || Aliases.TryGetValue(name, out command) ? command : null;

        private static void RegisterCommands()
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (!type.IsSubclassOf(typeof(CommandBase)) || type.IsAbstract)
                    continue;
                
                var atrs = type.GetCustomAttributes(false);

                foreach (var attr in atrs)
                {
                    if (attr is not CommandHandlerAttribute atr)
                        continue;
                    
                    var c = (CommandBase)Activator.CreateInstance(type);
                    c.SetData(atr.Name, atr.Description);

                    foreach (var attrx in atrs)
                        if (attrx is CommandAliasAttribute atrx)
                            c.Aliases.Add(atrx.Alias);
                        else if (attrx is ConfidentialCommand)
                            c.Confidential = true;

                    RegisterCommand(c);
                    break;
                }
            }
        }

        private static void HandleInput(string input)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(input))
                    return;
            
                var split = input.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                var name = split[0].ToLowerInvariant();
                var command = GetCommandByName(name);

                if (command != null)
                {
                    if (command.Confidential)
                    {
                        Logger.Log(input, Logger.LogEntryPriority.CommandInput, type: Logger.LogType.Print);
                        Logger.Log($"{name} [REDACTED]", Logger.LogEntryPriority.CommandInput, type: Logger.LogType.Log);
                    }
                    else Logger.Log(input, Logger.LogEntryPriority.CommandInput);
                    
                    try
                    {
                        command.Execute(split.Skip(1).ToArray());
                    }
                    catch (Exception e)
                    {
                        Logger.Log($"Failed to execute command {command}: {e.Message}", Logger.LogEntryPriority.Error);
                    }
                }
                else
                {
                    Logger.Log(input, Logger.LogEntryPriority.CommandInput);
                    Logger.Log("Invalid command.", Logger.LogEntryPriority.Error);
                }
            }
            catch (Exception e)
            {
                Logger.Log($"Processing server console input failed: {e.Message}", Logger.LogEntryPriority.Error);
            }
        }
    }
    
    public abstract class CommandBase
    {
        internal string Name, Description;
        internal bool Confidential;
        internal readonly List<string> Aliases = new();

        public void SetData(string name, string description)
        {
            Name = name.ToLowerInvariant();
            Description = description;
        }

        internal abstract void Execute(string[] arguments);
    }

    [AttributeUsage(AttributeTargets.Class)]
    internal class CommandHandlerAttribute : Attribute
    {
        internal readonly string Name, Description;

        internal CommandHandlerAttribute(string name, string description)
        {
            Name = name.ToLowerInvariant();
            Description = description;
        }
    }
    
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    internal class CommandAliasAttribute : Attribute
    {
        internal readonly string Alias;

        internal CommandAliasAttribute(string alias) => Alias = alias;
    }
    
    [AttributeUsage(AttributeTargets.Class)]
    internal class ConfidentialCommand : Attribute
    {
    }
}