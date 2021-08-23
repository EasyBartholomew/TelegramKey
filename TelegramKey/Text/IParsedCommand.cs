using System.Collections.Generic;


namespace TelegramKey.Text
{
    public interface IParsedCommand
    {
        string BotName { get; }

        string Name { get; }

        CommandType CommandType { get; }

        IReadOnlyList<ICommandArgument> Arguments { get; }
    }
}