using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramKey.Bot.Helpers;
using TelegramKey.Text;


namespace TelegramKey.Bot
{
    internal class DataManager : IDataManager
    {
        public ITelegramBotClient Client { get; }

        public List<Message> MessageReceiver { get; }

        public List<CallbackQuery> CallbackQueryReceiver { get; }

        public List<AwaitableMessage> AwaitableMessages { get; }

        public List<AwaitableQuery> AwaitableCallbackQueries { get; }


        public DataManager(ITelegramBotClient client)
        {
            this.Client                   = client;
            this.MessageReceiver          = new List<Message>();
            this.CallbackQueryReceiver    = new List<CallbackQuery>();
            this.AwaitableMessages        = new List<AwaitableMessage>();
            this.AwaitableCallbackQueries = new List<AwaitableQuery>();
        }

        public Task<Message> SendAsync(
            ChatId            chatId,
            string            text,
            ParseMode?        parseMode                = null,
            bool              disableWebPagePreview    = false,
            bool              disableNotification      = true,
            int?              replyToMessageId         = null,
            bool              allowSendingWithoutReply = true,
            IReplyMarkup      replyMarkup              = null,
            CancellationToken cancellationToken        = default)
        {
            return this.Client.SendTextMessageAsync(
                chatId,
                text,
                parseMode,
                null,
                disableWebPagePreview,
                disableNotification,
                replyToMessageId,
                allowSendingWithoutReply,
                replyMarkup,
                cancellationToken);
        }

        public Task DeleteMessageAsync(
            ChatId            chatId,
            int               messageId,
            CancellationToken cancellationToken = default)
        {
            return this.Client.DeleteMessageAsync(chatId, messageId, cancellationToken);
        }

        public Task<Message> EditMessageAsync(
            ChatId               chatId,
            int                  messageId,
            string               text,
            ParseMode?           parseMode             = null,
            bool                 disableWebPagePreview = false,
            InlineKeyboardMarkup replyMarkup           = null,
            CancellationToken    cancellationToken     = default)
        {
            return this.Client.EditMessageTextAsync(
                chatId,
                messageId,
                text,
                parseMode,
                disableWebPagePreview: disableWebPagePreview,
                replyMarkup: replyMarkup,
                cancellationToken: cancellationToken);
        }

        public Task<Message> EditMessageReplyMarkupAsync(
            ChatId               chatId,
            int                  messageId,
            InlineKeyboardMarkup replyMarkup       = null,
            CancellationToken    cancellationToken = default)
        {
            return this.Client.EditMessageReplyMarkupAsync(
                chatId,
                messageId,
                replyMarkup,
                cancellationToken);
        }

        public Task PinChatMessageAsync(
            ChatId            chatId,
            int               messageId,
            bool              disableNotification = false,
            CancellationToken cancellationToken   = default)
        {
            return this.Client.PinChatMessageAsync(
                chatId,
                messageId,
                disableNotification,
                cancellationToken);
        }

        public Task AnswerCallbackQueryAsync(
            string            callbackQueryId,
            string            text              = null,
            bool?             showAlert         = null,
            string            url               = null,
            int?              cacheTime         = null,
            CancellationToken cancellationToken = default)
        {
            return this.Client.AnswerCallbackQueryAsync(
                callbackQueryId,
                text,
                showAlert,
                url,
                cacheTime,
                cancellationToken);
        }

        public Task UnpinChatMessageAsync(
            ChatId            chatId,
            int               messageId,
            CancellationToken cancellationToken = default)
        {
            return this.Client.UnpinChatMessageAsync(
                chatId,
                messageId,
                cancellationToken);
        }

        public Task UnpinAllChatMessages(
            ChatId            chatId,
            CancellationToken cancellationToken = default)
        {
            return this.Client.UnpinAllChatMessages(chatId, cancellationToken);
        }

        public Task RestrictChatMemberAsync(
            ChatId            chatId,
            long              userId,
            ChatPermissions   permissions,
            DateTime?         untilDate         = null,
            CancellationToken cancellationToken = default)
        {
            return this.Client.RestrictChatMemberAsync(
                chatId,
                userId,
                permissions,
                untilDate,
                cancellationToken);
        }

        public Task<Message> AwaitMessageAsync(
            ChatId            chatId,
            User              user              = null,
            MessageType       messageType       = MessageType.Text,
            CancellationToken cancellationToken = default)
        {
            var awaitable = new AwaitableMessage(user, chatId, messageType, TextType.None);
            this.AwaitableMessages.Add(awaitable);

            Message message = null;

            while (message == null)
            {
                message = this.MessageReceiver.ToList().FirstOrDefault(
                    m => m.Chat.Id == awaitable.ChatId
                      && awaitable.User.NullOrEquals(awaitable.User?.Id, m.From?.Id)
                      && m.Type == awaitable.MessageType);

                if (cancellationToken.IsCancellationRequested)
                {
                    this.AwaitableMessages.Remove(awaitable);
                    cancellationToken.ThrowIfCancellationRequested();
                }

                if (message == null)
                    Thread.Sleep(50);
            }

            this.MessageReceiver.Remove(message);
            this.AwaitableMessages.Remove(awaitable);

            return Task.FromResult(message);
        }

        public Task<Message> AwaitTextMessageAsync(
            ChatId            chatId,
            User              user              = null,
            TextType          textType          = TextType.Text,
            CancellationToken cancellationToken = default)
        {
            var awaitable = new AwaitableMessage(user, chatId, MessageType.Text, textType);
            this.AwaitableMessages.Add(awaitable);

            Message message    = null;
            var     textParser = TextParserFactory.Create();

            while (message == null)
            {
                message = this.MessageReceiver.ToList().FirstOrDefault(
                    m => m.Chat.Id == awaitable.ChatId
                      && awaitable.User.NullOrEquals(awaitable.User?.Id, m.From?.Id)
                      && m.Type == awaitable.MessageType
                      && textParser.Parse(m.Text) == textType);

                if (cancellationToken.IsCancellationRequested)
                {
                    this.AwaitableMessages.Remove(awaitable);
                    cancellationToken.ThrowIfCancellationRequested();
                }

                if (message == null)
                    Thread.Sleep(50);
            }

            this.MessageReceiver.Remove(message);
            this.AwaitableMessages.Remove(awaitable);

            return Task.FromResult(message);
        }

        public async Task<CallbackQuery> AwaitQueryAsync(
            ChatId            chatId,
            int               messageId,
            User              user              = null,
            CancellationToken cancellationToken = default)
        {
            var awaitable = new AwaitableQuery(user, chatId, messageId);
            this.AwaitableCallbackQueries.Add(awaitable);

            CallbackQuery query = null;

            while (query == null)
            {
                query = this.CallbackQueryReceiver
                            .ToList()
                            .FirstOrDefault(
                                q =>
                                    q?.Message?.MessageId == messageId
                                 && awaitable.User.NullOrEquals(
                                        awaitable.User?.Id,
                                        q.From.Id));

                if (cancellationToken.IsCancellationRequested)
                {
                    this.AwaitableCallbackQueries.Remove(awaitable);
                    this.CallbackQueryReceiver.RemoveAll(q => q.Message?.MessageId == messageId);

                    // ReSharper disable once MethodSupportsCancellation
                    await this.DeleteMessageAsync(chatId, messageId);
                    cancellationToken.ThrowIfCancellationRequested();
                }

                if (query == null)
                    Thread.Sleep(50);
            }

            this.AwaitableCallbackQueries.Remove(awaitable);
            this.CallbackQueryReceiver.RemoveAll(q => q.Message?.MessageId == messageId);
            return query;
        }

        public async Task<CallbackQuery> AwaitDialogAsync<T>(
            ChatId            chatId,
            string            message,
            IEnumerable<T>    options,
            Func<T, string>   textProperty,
            Func<T, string>   dataProperty,
            bool              horizontal        = false,
            bool              deleteOnComplete  = true,
            ParseMode?        parseMode         = null,
            int?              replyToMessageId  = null,
            User              user              = null,
            CancellationToken cancellationToken = default)
        {
            InlineKeyboardMarkup markup;

            if (horizontal)
            {
                markup = new InlineKeyboardMarkup(
                    options.ToList().Select(o => new InlineKeyboardButton(textProperty(o))
                                                 {CallbackData = dataProperty(o)}));
            }
            else
            {
                markup = new InlineKeyboardMarkup(
                    options.Select(o => new[]
                                        {
                                            new InlineKeyboardButton(textProperty(o))
                                            {CallbackData = dataProperty(o)}
                                        }));
            }

            var queryMessage = await this.SendAsync(
                chatId,
                message,
                parseMode,
                replyToMessageId: replyToMessageId,
                replyMarkup: markup,
                cancellationToken: cancellationToken);

            var result = await this.AwaitQueryAsync(
                chatId,
                queryMessage.MessageId,
                user,
                cancellationToken);

            if (deleteOnComplete)
                await this.DeleteMessageAsync(chatId, queryMessage.MessageId);

            return result;
        }

        public async Task<CallbackQuery> AwaitDialogAsync<T>(
            ChatId            chatId,
            string            message,
            IEnumerable<T>    options,
            Func<T, string>   textProperty,
            bool              horizontal        = false,
            bool              deleteOnComplete  = true,
            ParseMode?        parseMode         = null,
            int?              replyToMessageId  = null,
            User              user              = null,
            CancellationToken cancellationToken = default)
        {
            return await this.AwaitDialogAsync(
                chatId,
                message,
                options,
                textProperty,
                textProperty,
                horizontal,
                deleteOnComplete,
                parseMode,
                replyToMessageId,
                user,
                cancellationToken);
        }

        public async Task<CallbackQuery> AwaitDialogAsync(
            ChatId              chatId,
            string              message,
            IEnumerable<string> options,
            bool                horizontal        = false,
            bool                deleteOnComplete  = true,
            ParseMode?          parseMode         = null,
            int?                replyToMessageId  = null,
            User                user              = null,
            CancellationToken   cancellationToken = default)
        {
            return await this.AwaitDialogAsync(
                chatId,
                message,
                options,
                x => x,
                horizontal,
                deleteOnComplete,
                parseMode,
                replyToMessageId,
                user,
                cancellationToken);
        }
    }
}