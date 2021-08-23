using System.Collections.Generic;
using System.Linq;


namespace TelegramKey.Text.Implementation
{
    internal class ParsedCommand : IParsedCommand
    {
        public string BotName { get; set; }

        public string Name { get; }

        public CommandType CommandType { get; }

        public List<ICommandArgument> Arguments { get; }

        IReadOnlyList<ICommandArgument> IParsedCommand.Arguments =>
            this.Arguments.ToList().AsReadOnly();

        public ParsedCommand(string name, CommandType commandType, string botName = null)
        {
            this.Name        = name;
            this.BotName     = botName;
            this.CommandType = commandType;
            this.Arguments   = new List<ICommandArgument>();
        }
    }
}