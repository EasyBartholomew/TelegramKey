using System;
using TelegramKey.Text.Implementation;


namespace TelegramKey.Text
{
    public static class TextParserFactory
    {
        public static ITextCommandParser Create(TextType type)
        {
            switch (type)
            {
                case TextType.Command:
                    return new TextCommandParser();
                case TextType.MentionCommand:
                    return new MentionTextCommandParser();

                default:
                    throw new ArgumentException("", nameof(type));
            }
        }

        public static ITextParser Create()
        {
            return new TextParser();
        }
    }
}