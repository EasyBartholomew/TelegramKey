using System;
using System.Linq;
using System.Text.RegularExpressions;


namespace TelegramKey.Text.Implementation
{
    internal class TextCommandParser : ITextCommandParser
    {
        public IParsedCommand Parse(string text)
        {
            ITextParser textParser = new TextParser();

            var type = textParser.Parse(text);

            if (type != TextType.Command)
                return null;

            var match = Regex
                        .Match(text, @"^\/(\w+)(@(\w+))?")
                        .Value;

            ParsedCommand command;

            if (match.Contains("@"))
            {
                var commandAndBot = match.Replace("/", "")
                                         .Split(new[] {'@'},
                                                StringSplitOptions.RemoveEmptyEntries);

                command = new ParsedCommand(
                    commandAndBot[0],
                    CommandType.Explicit,
                    commandAndBot[1]);
            }
            else
            {
                command = new ParsedCommand(
                    match.Replace("/", ""),
                    CommandType.Implicit);
            }

            var args = text.Replace(match, "");

            command.Arguments.AddRange(
                args.Split(
                        new[] {'\n', ','},
                        StringSplitOptions.RemoveEmptyEntries)
                    .Select(a => new CommandArgument(a.Trim())));

            return command;
        }
    }
}