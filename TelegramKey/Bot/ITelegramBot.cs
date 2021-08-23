using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;


namespace TelegramKey.Bot
{
    public interface ITelegramBot
    {
        IDataManager DataManager { get; }

        string Name { get; }

        IList<ICommand> Commands { get; }

        Task SetMyCommandsAsync(IEnumerable<ICommand> botCommands);

        Task SetMyCommandsForScopeAsync(
            IEnumerable<ICommand> botCommands,
            BotCommandScope       commandScope = null);

        void Start();

        void Stop();
    }
}