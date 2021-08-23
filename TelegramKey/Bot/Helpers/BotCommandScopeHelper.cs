using System;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;


namespace TelegramKey.Bot.Helpers
{
    public static class BotCommandScopeHelper
    {
        public static BotCommandScope CreateScope(this BotCommandScopeType scopeType)
        {
            switch (scopeType)
            {
                case BotCommandScopeType.Default:
                    return BotCommandScope.Default();

                case BotCommandScopeType.AllPrivateChats:
                    return BotCommandScope.AllPrivateChats();

                case BotCommandScopeType.AllGroupChats:
                    return BotCommandScope.AllGroupChats();

                case BotCommandScopeType.AllChatAdministrators:
                    return BotCommandScope.AllChatAdministrators();

                default:
                    throw new InvalidOperationException();
            }
        }
    }
}