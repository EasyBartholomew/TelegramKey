using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;


namespace TelegramKey.Bot
{
    public class UpdateDaemon : IUpdateDaemon
    {
        protected ITelegramBotClient Client { get; }

        public IReceiver<Update> Receiver { get; }

        public bool IsStarted { get; protected set; }

        public UpdateDaemon(ITelegramBotClient client, IReceiver<Update> receiver)
        {
            this.IsStarted = false;
            this.Client    = client;
            this.Receiver  = receiver;
        }

        public void Start()
        {
            this.IsStarted = true;
            Task.Factory.StartNew(this.Poll);
        }

        public void Stop()
        {
            this.IsStarted = false;
        }

        private async void Poll()
        {
            Update[] updates = null;

            while (this.IsStarted)
            {
                updates = await this.Client.GetUpdatesAsync(
                    updates != null && updates.Length != 0 ? updates.Last().Id + 1 : 0,
                    100,
                    50,
                    new[]
                    {
                        UpdateType.Message,
                        UpdateType.CallbackQuery
                    });

                this.Receiver.Receive(updates);
            }
        }
    }
}