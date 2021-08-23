using Telegram.Bot.Types;


namespace TelegramKey.Bot
{
    public interface IUpdateDaemon
    {
        IReceiver<Update> Receiver { get; }

        bool IsStarted { get; }

        void Start();

        void Stop();
    }
}