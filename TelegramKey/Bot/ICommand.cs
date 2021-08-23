using System.Collections.Generic;
using Telegram.Bot.Types.Enums;
using TelegramKey.Bot.Commands;


namespace TelegramKey.Bot
{
    public interface ICommand
    {
        string Name { get; }

        ISet<ICommandDescription> Descriptions { get; }

        bool RequireInit { get; }

        bool RequireExplicit { get; }

        ICommandHandler CreateHandler(ChatType chatType);

        ISet<ChatMemberStatus> WhoCanExecute { get; }

        ICommandDescription CreateDescription(
            string              descriptionText = null,
            string              languageCode    = null,
            BotCommandScopeType scopeType       = BotCommandScopeType.Default);

        void DestroyDescription(ICommandDescription description);
    }
}