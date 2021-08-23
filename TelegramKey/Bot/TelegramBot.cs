using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramKey.Bot.Helpers;
using TelegramKey.Text;


namespace TelegramKey.Bot
{
    internal class TelegramBot : ITelegramBot
    {
        private readonly DataManager _dataManager;

        public ITelegramBotClient Client { get; }

        protected IReceiver<Update> Receiver { get; }

        protected List<UserSession> UserSessions { get; }

        protected bool IsWorking { get; set; }

        public IDataManager DataManager => this._dataManager;

        public string Name { get; private set; }


        public IList<ICommand> Commands { get; }

        public TelegramBot(ITelegramBotClient client, IReceiver<Update> receiver)
        {
            this.Receiver     = receiver;
            this.Commands     = new List<ICommand>();
            this.Client       = client;
            this._dataManager = new DataManager(this.Client);
            this.UserSessions = new List<UserSession>();
            this.IsWorking    = false;
        }


        public async Task SetMyCommandsForScopeAsync(
            IEnumerable<ICommand> botCommands,
            BotCommandScope       commandScope = null)
        {
            if (commandScope == null)
                return;

            var commandsData = botCommands
                               .SelectMany(c => c.Descriptions)
                               .Where(d => d.ScopeType
                                            .IsOneOf(
                                                commandScope.Type))
                               .Select(d =>
                                           new
                                           {
                                               Command = new BotCommand()
                                                         {Command = d.Command.Name, Description = d.Description},
                                               d.LanguageCode,
                                               d.ScopeType
                                           })
                               .GroupBy(x => x.ScopeType);


            foreach (var commandData in commandsData)
            {
                var commandsLang = commandData
                    .GroupBy(x => x.LanguageCode);

                foreach (var c in commandsLang)
                    await this.Client.SetMyCommandsAsync(
                        c.Select(x => x.Command),
                        commandData.Key.CreateScope(),
                        c.Key);
            }
        }

        public async Task SetMyCommandsAsync(IEnumerable<ICommand> botCommands)
        {
            var commandsData = botCommands
                               .SelectMany(c => c.Descriptions)
                               .Where(d => d.ScopeType
                                            .IsNotOneOf(
                                                BotCommandScopeType.Chat,
                                                BotCommandScopeType.ChatMember,
                                                BotCommandScopeType.ChatAdministrators))
                               .Select(d =>
                                           new
                                           {
                                               Command = new BotCommand()
                                                         {Command = d.Command.Name, Description = d.Description},
                                               d.LanguageCode,
                                               d.ScopeType
                                           })
                               .GroupBy(x => x.ScopeType);


            foreach (var commandData in commandsData)
            {
                var commandsLang = commandData
                    .GroupBy(x => x.LanguageCode);

                foreach (var c in commandsLang)
                    await this.Client.SetMyCommandsAsync(
                        c.Select(x => x.Command),
                        commandData.Key.CreateScope(),
                        c.Key);
            }
        }

        public async void Start()
        {
            var me = await this.Client.GetMeAsync();
            this.Name = me.Username;

            this.IsWorking = true;

            while (this.IsWorking)
            {
                var update = await this.Receiver.GetAsync();

                switch (update.Type)
                {
                    case UpdateType.Message:
                        ThreadPool.QueueUserWorkItem(this.OnNewMessage, update.Message);
                        break;
                    case UpdateType.CallbackQuery:
                        ThreadPool.QueueUserWorkItem(this.OnNewCallbackQuery, update.CallbackQuery);
                        break;
                }
            }
        }

        public void Stop()
        {
            this.IsWorking = false;
        }

        private void OnNewMessage(object state)
        {
            if (!(state is Message message))
                return;

            switch (message.Type)
            {
                case MessageType.Text:
                    this.OnNewTextMessage(message);
                    break;

                case MessageType.Unknown:
                    break;

                case MessageType.Photo:
                    break;
                case MessageType.Audio:
                    break;
                case MessageType.Video:
                    break;
                case MessageType.Voice:
                    break;
                case MessageType.Document:
                    break;
                case MessageType.Sticker:
                    break;
                case MessageType.Location:
                    break;
                case MessageType.Contact:
                    break;
                case MessageType.Venue:
                    break;
                case MessageType.Game:
                    break;
                case MessageType.VideoNote:
                    break;
                case MessageType.Invoice:
                    break;
                case MessageType.SuccessfulPayment:
                    break;
                case MessageType.WebsiteConnected:
                    break;
                case MessageType.ChatMembersAdded:
                    break;
                case MessageType.ChatMemberLeft:
                    break;
                case MessageType.ChatTitleChanged:
                    break;
                case MessageType.ChatPhotoChanged:
                    break;
                case MessageType.MessagePinned:
                    break;
                case MessageType.ChatPhotoDeleted:
                    break;
                case MessageType.GroupCreated:
                    break;
                case MessageType.SupergroupCreated:
                    break;
                case MessageType.ChannelCreated:
                    break;
                case MessageType.MigratedToSupergroup:
                    break;
                case MessageType.MigratedFromGroup:
                    break;
                case MessageType.Poll:
                    break;
                case MessageType.Dice:
                    break;
                case MessageType.MessageAutoDeleteTimerChanged:
                    break;
                case MessageType.ProximityAlertTriggered:
                    break;
                case MessageType.VoiceChatScheduled:
                    break;
                case MessageType.VoiceChatStarted:
                    break;
                case MessageType.VoiceChatEnded:
                    break;
                case MessageType.VoiceChatParticipantsInvited:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnNewTextMessage(Message message)
        {
            var textParser = TextParserFactory.Create();
            var textType   = textParser.Parse(message.Text);

            switch (textType)
            {
                case TextType.Command:
                case TextType.MentionCommand:
                    this.OnNewCommand(message, textType);
                    break;

                case TextType.Mention:
                case TextType.Text:
                    this.OnNewTextOrMention(message, textType);
                    break;

                default:
                    throw new InvalidOperationException();
            }
        }

        private readonly object _commandLock = new object();

        private async void OnNewCommand(Message message, TextType messageType)
        {
            var commandParser = TextParserFactory.Create(messageType);
            var parsedCommand = commandParser.Parse(message.Text);

            if (parsedCommand.CommandType == CommandType.Explicit)
                if (this.Name != parsedCommand.BotName)
                    return;

            var command    = this.Commands.FirstOrDefault(c => c.Name == parsedCommand.Name);
            var chatMember = await this.Client.GetChatMemberAsync(message.Chat.Id, message.From.Id);
            var chat       = message.Chat;

            if (command == null)
                return;

            if (chat.Type.IsNotOneOf(ChatType.Private, ChatType.Sender))
            {
                if (command.RequireExplicit && parsedCommand.CommandType != CommandType.Explicit)
                    return;

                if (!command.WhoCanExecute.Contains(chatMember.Status))
                    return;
            }

            var handler = command.CreateHandler(chat.Type);

            if (handler == null)
                return; // add call OnWrongCommand

            Task task;

            var userSession = new UserSession(message.Chat.Id, message.From.Id);

            lock (this._commandLock)
            {
                var prevSession = this.UserSessions.FirstOrDefault(
                    s =>
                        s.ChatId == message.Chat.Id && s.UserId == message.From.Id);

                prevSession?.TokenSource.Cancel();
                this.UserSessions.Add(userSession);

                while (this.UserSessions.Contains(prevSession) ||
                       (prevSession?.Task?.Status
                                   .IsNotOneOf(
                                       TaskStatus.Canceled,
                                       TaskStatus.Faulted,
                                       TaskStatus.RanToCompletion) ?? false))
                { }

                task = handler
                    .Handle(
                        this,
                        parsedCommand,
                        message,
                        userSession.TokenSource.Token);

                userSession.Task = task;
            }

            try
            {
                userSession.TokenSource.CancelAfter(TimeSpan.FromMinutes(5));
                task.Wait();
            }
            catch (AggregateException ex)
            {
                if (ex.InnerExceptions.Count != 1)
                    throw;
            }
            finally
            {
                this.UserSessions.Remove(userSession);
                userSession.Dispose();
            }
        }

        private void OnNewTextOrMention(Message message, TextType textType)
        {
            Func<AwaitableMessage, bool> findDelegate = m =>
                m.User.NullOrEquals(m.User?.Id, message.From?.Id)
             && m.ChatId == message.Chat.Id
             && m.MessageType == message.Type
             && m.TextType == textType;

            if (this._dataManager.AwaitableMessages.Count(findDelegate) != 0)
                this._dataManager.MessageReceiver.Add(message);

            switch (textType)
            {
                case TextType.Mention:
                    this.OnNewMention(message);
                    break;

                case TextType.Text:
                    this.OnNewText(message);
                    break;
            }
        }

        private void OnNewMention(Message message)
        { }

        private void OnNewText(Message message)
        { }

        private void OnNewCallbackQuery(object state)
        {
            if (!(state is CallbackQuery query))
                return;

            Func<AwaitableQuery, bool> selector = x =>
                x.ChatId == query.Message?.Chat.Id
             && x.User.NullOrEquals(x.User?.Id, query.From.Id)
             && x.MessageId == query.Message?.MessageId;

            if (this._dataManager.AwaitableCallbackQueries.Any(selector))
                this._dataManager.CallbackQueryReceiver.Add(query);
        }
    }
}