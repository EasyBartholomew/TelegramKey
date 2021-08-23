namespace TelegramKey.Text
{
    public interface ITextCommandParser
    {
        IParsedCommand Parse(string text);
    }
}