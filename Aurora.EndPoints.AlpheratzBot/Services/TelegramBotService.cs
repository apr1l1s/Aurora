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
    ISubscribersRepository subscribersRepository,
    ILogger<TelegramBotService> logger)
    : IUpdateHandler, ITelegramBotService
{
    public void StartBotAsync(string token)
    {
        telegramBot.StartReceiving(
            updateHandler:HandleUpdateAsync,
            errorHandler: HandleErrorAsync);
        Console.WriteLine("Bot is receiving messages.");
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, 
        CancellationToken cancellationToken)
    {
        var message = update.Message;
        if (message == null)
        {
            return;
        }

        var sub = new Subscriber(message);
        var command = message.Text;

        switch (command)
        {
            case "/sub":
                if (subscribersRepository.AddItem(sub))
                {
                    await telegramBot.SendMessage(message.Chat.Id, "Лайк, подписка",
                        messageThreadId: message.MessageThreadId, cancellationToken: cancellationToken);
                    logger.LogInformation($"New subscriber: {message.Chat.Id}");
                }

                break;

            case "/unsub":
                if (subscribersRepository.RemoveItem(message.Chat.Id) > 0)
                {
                    await telegramBot.SendMessage(message.Chat.Id, "Понял, вычеркиваю.",
                        cancellationToken: cancellationToken);
                    logger.LogInformation($"Unsubscribed: {message.Chat.Id}");
                }

                break;

            default:
                if (command != null)
                {
                    await telegramBot.SendMessage(message.Chat.Id, command, cancellationToken: cancellationToken);
                }
                logger.LogInformation($"Unknown message: {message.Text}");

                break;
        }
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source,
        CancellationToken cancellationToken)
    {
        logger.LogInformation($"Unknown error: {exception.Message}");
        return Task.CompletedTask;
    }
}