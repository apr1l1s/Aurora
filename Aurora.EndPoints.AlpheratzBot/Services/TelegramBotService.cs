using Aurora.EndPoints.BorealisBot.Services;
using Core.Providers.Cqrs;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Aurora.EndPoints.AlpheratzBot.Services;

public class TelegramBotService(
    TelegramBotClient telegramBot,
    IUseCaseDispatcher dispatcher,
    ILogger<TelegramBotService> logger)
    : IUpdateHandler, ITelegramBotService
{
    public void StartBotAsync(string token)
    {
        telegramBot.StartReceiving(
            updateHandler: HandleUpdateAsync,
            errorHandler: HandleErrorAsync);
        logger.LogInformation("Bot is receiving messages.");
    }

    public Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
        => Task.Run(async () =>
        {
            var message = update.Message;
            if (message == null)
            {
                return;
            }

            var command = message.Text;

            await dispatcher.DispatchAsync(new CommandUseCase(command, message), cancellationToken);
        }, cancellationToken);

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source,
        CancellationToken cancellationToken)
    {
        logger.LogInformation($"Unknown error: {exception.Message}");

        return Task.CompletedTask;
    }
}