using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;


namespace TelegramKey.Bot
{
    public class UserSession : IDisposable
    {
        public ChatId ChatId { get; }

        public long UserId { get; }

        public CancellationTokenSource TokenSource { get; }

        public Task Task { get; set; }

        protected UserSession(ChatId chatId, long userId, CancellationTokenSource source)
        {
            this.ChatId      = chatId;
            this.UserId      = userId;
            this.TokenSource = source;
        }

        public UserSession(ChatId chatId, long userId)
            : this(chatId, userId, new CancellationTokenSource())
        { }

        public void Dispose()
        {
            this.TokenSource?.Dispose();
        }
    }
}