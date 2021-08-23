using Telegram.Bot.Types.Enums;

namespace TelegramKey.Bot.Commands
{
    internal class CommandDescription : ICommandDescription
    {
        private string _description;

        public string LanguageCode { get; }

        public ICommand Command { get; }

        public BotCommandScopeType ScopeType { get; set; }

        public string Description
        {
            get => this._description;
            set => this._description = string.IsNullOrWhiteSpace(value)
                ? "no description"
                : value.ToLower().Trim();
        }

        public CommandDescription(ICommand command, string languageCode, BotCommandScopeType scopeType)
        {
            this.ScopeType    = scopeType;
            this.Command      = command;
            this.LanguageCode = languageCode;
            this.Description  = "no description";
        }

        public CommandDescription(ICommand command, string languageCode)
            : this(command, languageCode, BotCommandScopeType.Default)
        { }

        public CommandDescription(ICommand command) : this(command, null)
        { }
    }
}