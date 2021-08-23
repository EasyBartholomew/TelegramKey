namespace TelegramKey.Text
{
    public interface ITextParser
    {
        TextType Parse(string text);
    }
}