﻿using Core.Providers.Cqrs;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Aurora.EndPoints.BorealisBot.Services;

public record Subscriber(long ChannelId, string Title, int? TopicId = null)
{
    public Subscriber(Message message)
        : this(message.Chat.Id, message.Chat.Title, message.MessageThreadId)
    {
    }
}

public class TelegramBotService(TelegramBotClient telegramBot,
    IUseCaseDispatcher dispatcher,
    ILogger<TelegramBotService> logger)
    : IUpdateHandler, ITelegramBotService
{
    public void StartBotAsync(string token)
    {
        telegramBot.StartReceiving(
            updateHandler:HandleUpdateAsync,
            errorHandler: HandleErrorAsync);
        logger.LogInformation("Bot is receiving messages.");
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, 
        CancellationToken cancellationToken)
    {
        var message = update.Message;
        if (message == null)
        {
            return;
        }

        var command = message.Text;

        await dispatcher.DispatchAsync(new CommandUseCase(command, message), cancellationToken);
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source,
        CancellationToken cancellationToken)
    {
        logger.LogInformation($"Unknown error: {exception.Message}");

        return Task.CompletedTask;
    }
}