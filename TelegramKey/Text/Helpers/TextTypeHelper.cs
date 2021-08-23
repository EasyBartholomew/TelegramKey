namespace TelegramKey.Text.Helpers
{
    public static class TextTypeHelper
    {
        public static bool IsCommand(this TextType type)
        {
            switch (type)
            {
                case TextType.Command:
                case TextType.MentionCommand:
                    return true;

                default:
                    return false;
            }
        }
    }
}