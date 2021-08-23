using System;
using System.Linq;
using System.Text.RegularExpressions;


namespace TelegramKey.Text.Implementation
{
    internal class MentionTextCommandParser : ITextCommandParser
    {
        public IParsedCommand Parse(string text)
        {
            ITextParser textParser = new TextParser();

            var type = textParser.Parse(text);

            if (type != TextType.MentionCommand)
                return null;

            var botName = Regex.Match(text, @"^@\w+")
                               .Value;

            var commandName = text
                              .Replace(botName + " ", "")
                              .Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)
                              .First().Trim();

            var command = new ParsedCommand(
                commandName,
                CommandType.Explicit,
                botName.Replace("@", ""));

            command.Arguments.AddRange(
                text
                    .Replace(botName, "")
                    .Replace(commandName, "")
                    .Trim()
                    .Split(new[] {',', '\n'}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(a => new CommandArgument(a.Trim())));

            return command;
        }
    }
}