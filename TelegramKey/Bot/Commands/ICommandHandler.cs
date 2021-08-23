using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramKey.Text;


namespace TelegramKey.Bot.Commands
{
    public interface ICommandHandler
    {
        Task Handle(
            ITelegramBot      bot,
            IParsedCommand    command,
            Message           message,
            CancellationToken cancellationToken);
    }
}