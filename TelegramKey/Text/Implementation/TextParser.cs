using System.Text.RegularExpressions;

namespace TelegramKey.Text.Implementation
{
    internal class TextParser : ITextParser
    {
        private const string CommandPattern = @"^\/(\w+)";

        private const string MentionPattern = @"^@(\w+)";

        private const string MentionCommandPattern = @"^(@\w+bot)\s+[\s\S]+";

        public TextParser()
        { }

        public TextType Parse(string text)
        {
            var type = Regex.IsMatch(text, CommandPattern)
                ? TextType.Command
                : TextType.Text;

            if (type == TextType.Text)
                type = Regex.IsMatch(text, MentionCommandPattern, RegexOptions.IgnoreCase)
                    ? TextType.MentionCommand
                    : TextType.Text;

            if (type == TextType.Text)
                type = Regex.IsMatch(text, MentionPattern)
                    ? TextType.Mention
                    : TextType.Text;

            return type;
        }
    }
}