using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramKey.Text;


namespace TelegramKey.Bot
{
    public class AwaitableMessage
    {
        public User User { get; }

        public ChatId ChatId { get; }

        public MessageType MessageType { get; }

        public TextType TextType { get; }

        public AwaitableMessage(User user, ChatId chatId, MessageType messageType, TextType textType)
        {
            this.User        = user;
            this.ChatId      = chatId;
            this.MessageType = messageType;
            this.TextType    = textType;
        }
    }
}