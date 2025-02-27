﻿using System.Text;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
using Zefirrat.YandexGpt.Abstractions;

namespace Aurora.EndPoints.SerpensBot.Services.Handlers;

public partial class UpdateHandler(ITelegramBotClient bot, ILogger<UpdateHandler> logger, IYaPrompter prompter) 
    : IUpdateHandler
{
    private static readonly InputPollOption[] PollOptions = ["Готов к покуру", "Через 5 мин го", "Ядавид"];

    public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
    {
        logger.LogInformation("HandleError: {Exception}", exception);
        // Cooldown in case of network connection error
        if (exception is RequestException)
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await (update switch
        {
            { Message: { } message }                        => OnMessage(message),
            { EditedMessage: { } message }                  => OnMessage(message),
            { CallbackQuery: { } callbackQuery }            => OnCallbackQuery(callbackQuery),
            { InlineQuery: { } inlineQuery }                => OnInlineQuery(inlineQuery),
            { ChosenInlineResult: { } chosenInlineResult }  => OnChosenInlineResult(chosenInlineResult),
            { Poll: { } poll }                              => OnPoll(poll),
            //{ PollAnswer: { } pollAnswer }                  => OnPollAnswer(pollAnswer),
            // UpdateType.ChannelPost:
            // UpdateType.EditedChannelPost:
            // UpdateType.ShippingQuery:
            // UpdateType.PreCheckoutQuery:
            _                                               => UnknownUpdateHandlerAsync(update)
        });
    }

    private async Task OnMessage(Message msg)
    {
        logger.LogInformation("Receive message type: {MessageType}", msg.Type);
        if (msg.Text is not { } messageText)
            return;

        var command = messageText.Split(' ', '@');
        if (command.Length > 1 && command[1] != "hydra_bank_alert_bot")
        {
            return;
        }

        var sentMessage = await (command[0] switch
        {
            "/alert" => SendAlert(msg),
            //"/photo" => SendPhoto(msg),
            //"/inline_buttons" => SendInlineKeyboard(msg),
            //"/keyboard" => SendReplyKeyboard(msg),
            //"/remove" => RemoveKeyboard(msg),
            //"/request" => RequestContactAndLocation(msg),
            //"/inline_mode" => StartInlineQuery(msg),
            "/poll" => SendPoll(msg),
            //"/poll_anonymous" => SendAnonymousPoll(msg),
            //"/throw" => FailingHandler(msg),
            "/git" => SendGit(msg),
            "/menu" => Usage(msg)
        });

        logger.LogInformation("The message was sent with id: {SentMessageId}", sentMessage.Id);
    }

    async Task<Message> SendGit(Message msg)
        => await bot.SendMessage(msg.Chat, "https://github.com/apr1l1s/Aurora");

    async Task<Message> Usage(Message msg)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Bot menu:");
        sb.AppendLine("/alert        - send alert");
        sb.AppendLine("/poll         - send poll");
        sb.AppendLine("/git         - send git project link");
        sb.AppendLine("/menu         - send menu");

        // const string usage = """
        //         <b><u>Bot menu</u></b>:
        //         /alert          - send alert
        //         /photo          - send a photo
        //         /inline_buttons - send inline buttons
        //         /keyboard       - send keyboard buttons
        //         /remove         - remove keyboard buttons
        //         /request        - request location or contact
        //         /inline_mode    - send inline-mode results list
        //         /poll           - send a poll
        //         /poll_anonymous - send an anonymous poll
        //         /throw          - what happens if handler fails
        //     """;
        return await bot.SendMessage(msg.Chat, sb.ToString(), replyMarkup: new ReplyKeyboardRemove());
    }

    async Task<Message> RequestContactAndLocation(Message msg)
    {
        var replyMarkup = new ReplyKeyboardMarkup(true)
            .AddButton(KeyboardButton.WithRequestLocation("Location"))
            .AddButton(KeyboardButton.WithRequestContact("Contact"));
        return await bot.SendMessage(msg.Chat, "Who or Where are you?", replyMarkup: replyMarkup);
    }

    static Task<Message> FailingHandler(Message msg)
    {
        throw new NotImplementedException("FailingHandler");
    }

    private Task UnknownUpdateHandlerAsync(Update update)
    {
        logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
        return Task.CompletedTask;
    }
}