using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramKey.Text;


namespace TelegramKey.Bot
{
    public interface IDataManager
    {
        Task<Message> SendAsync(
            ChatId            chatId,
            string            text,
            ParseMode?        parseMode                = null,
            bool              disableWebPagePreview    = false,
            bool              disableNotification      = true,
            int?              replyToMessageId         = null,
            bool              allowSendingWithoutReply = true,
            IReplyMarkup      replyMarkup              = null,
            CancellationToken cancellationToken        = default);

        Task DeleteMessageAsync(
            ChatId            chatId,
            int               messageId,
            CancellationToken cancellationToken = default);

        Task<Message> EditMessageAsync(
            ChatId               chatId,
            int                  messageId,
            string               text,
            ParseMode?           parseMode             = null,
            bool                 disableWebPagePreview = false,
            InlineKeyboardMarkup replyMarkup           = null,
            CancellationToken    cancellationToken     = default);

        Task<Message> EditMessageReplyMarkupAsync(
            ChatId               chatId,
            int                  messageId,
            InlineKeyboardMarkup replyMarkup       = null,
            CancellationToken    cancellationToken = default);

        Task PinChatMessageAsync(
            ChatId            chatId,
            int               messageId,
            bool              disableNotification = false,
            CancellationToken cancellationToken   = default);

        Task AnswerCallbackQueryAsync(
            string            callbackQueryId,
            string            text              = null,
            bool?             showAlert         = null,
            string            url               = null,
            int?              cacheTime         = null,
            CancellationToken cancellationToken = default);

        Task UnpinChatMessageAsync(
            ChatId            chatId,
            int               messageId,
            CancellationToken cancellationToken = default);

        Task UnpinAllChatMessages(
            ChatId            chatId,
            CancellationToken cancellationToken = default);

        Task RestrictChatMemberAsync(
            ChatId            chatId,
            long              userId,
            ChatPermissions   permissions,
            DateTime?         untilDate         = null,
            CancellationToken cancellationToken = default);

        Task<Message> AwaitMessageAsync(
            ChatId            chatId,
            User              user              = null,
            MessageType       messageType       = MessageType.Text,
            CancellationToken cancellationToken = default);

        Task<Message> AwaitTextMessageAsync(
            ChatId            chatId,
            User              user              = null,
            TextType          textType          = TextType.Text,
            CancellationToken cancellationToken = default);

        Task<CallbackQuery> AwaitQueryAsync(
            ChatId            chatId,
            int               messageId,
            User              user              = null,
            CancellationToken cancellationToken = default);

        Task<CallbackQuery> AwaitDialogAsync<T>(
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
            CancellationToken cancellationToken = default);

        Task<CallbackQuery> AwaitDialogAsync<T>(
            ChatId            chatId,
            string            message,
            IEnumerable<T>    options,
            Func<T, string>   textProperty,
            bool              horizontal        = false,
            bool              deleteOnComplete  = true,
            ParseMode?        parseMode         = null,
            int?              replyToMessageId  = null,
            User              user              = null,
            CancellationToken cancellationToken = default);

        Task<CallbackQuery> AwaitDialogAsync(
            ChatId              chatId,
            string              message,
            IEnumerable<string> options,
            bool                horizontal        = false,
            bool                deleteOnComplete  = true,
            ParseMode?          parseMode         = null,
            int?                replyToMessageId  = null,
            User                user              = null,
            CancellationToken   cancellationToken = default);
    }
}