using Telegram.Bot.Types;


namespace TelegramKey.Bot
{
    public class AwaitableQuery
    {
        public User User { get; }

        public ChatId ChatId { get; }

        public long MessageId { get; }

        public AwaitableQuery(User user, ChatId chatId, long messageId)
        {
            this.User      = user;
            this.ChatId    = chatId;
            this.MessageId = messageId;
        }
    }
}