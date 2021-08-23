using Telegram.Bot.Types.Enums;

namespace TelegramKey.Bot.Commands
{
    public interface ICommandDescription
    {
        ICommand Command { get; }

        string LanguageCode { get; }

        string Description { get; set; }

        BotCommandScopeType ScopeType { get; set; }
    }
}