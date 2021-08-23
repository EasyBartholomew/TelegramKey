using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Telegram.Bot.Types.Enums;
using TelegramKey.Bot.Helpers;


namespace TelegramKey.Bot.Commands
{
    public abstract class Command : ICommand
    {
        public virtual string Name { get; }

        public ISet<ICommandDescription> Descriptions { get; }

        public virtual bool RequireInit { get; protected set; }

        public virtual bool RequireExplicit { get; protected set; }

        public abstract ICommandHandler CreateHandler(ChatType chatType);

        public ISet<ChatMemberStatus> WhoCanExecute { get; }

        public ICommandDescription CreateDescription(
            string              descriptionText = null,
            string              languageCode    = null,
            BotCommandScopeType scopeType       = BotCommandScopeType.Default)
        {
            if (this.Descriptions.Any(d =>
                                          (d.LanguageCode == languageCode)
                                       && scopeType
                                              .IsNotOneOf(
                                                  BotCommandScopeType.AllGroupChats,
                                                  BotCommandScopeType.AllPrivateChats,
                                                  BotCommandScopeType.AllChatAdministrators,
                                                  BotCommandScopeType.Default)
                                       && (d.ScopeType == scopeType)))
                throw new ArgumentException();

            var description = new CommandDescription(this, languageCode, scopeType)
                              {Description = descriptionText};

            this.Descriptions.Add(description);

            return description;
        }

        public void DestroyDescription(ICommandDescription description)
        {
            if (description.Command != this)
                throw new ArgumentException();

            this.Descriptions.Remove(description);
        }

        protected Command(
            IEnumerable<ChatMemberStatus> whoCanExecute,
            bool                          requireInit = false)
        {
            var typename = this.GetType().Name;

            if (!typename.EndsWith(nameof(Command), StringComparison.InvariantCulture))
                throw new InvalidDataException();

            var name = typename
                       .Replace(nameof(Command), "")
                       .ToLower();

            this.Name         = name;
            this.Descriptions = new HashSet<ICommandDescription>();

            // ReSharper disable once VirtualMemberCallInConstructor
            this.RequireInit = requireInit;
            // ReSharper disable once VirtualMemberCallInConstructor
            this.RequireExplicit = false;
            this.WhoCanExecute   = new HashSet<ChatMemberStatus>(whoCanExecute);
        }

        protected Command()
            : this(new[]
                   {ChatMemberStatus.Creator, ChatMemberStatus.Administrator})
        { }
    }
}