using Telegram.Bot;
using Telegram.Bot.Types;


namespace TelegramKey.Bot
{
    public static class TelegramBotFactory
    {
        public static ITelegramBot Create(ITelegramBotClient client, IReceiver<Update> updateReceiver)
        {
            return new TelegramBot(client, updateReceiver);
        }
    }
}