using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace Aurora.EndPoints.BorealisBot.Services;

public class RandomMessageScheduler(TelegramBotClient telegramBot,
    ISubscribersRepository subscribersRepository,
    ILogger<RandomMessageScheduler> logger)
: MessageScheduler
{
    private readonly Random _random = new();

    protected override async Task OnMessage()
    {
        while (true)
        {
            await WaitDelay();

            const string message = "Андрей - гей";

            var subscribers = subscribersRepository.GetCollection();
            foreach (var sub in subscribers)
            {
                await telegramBot.SendMessage(sub.ChannelId, message, messageThreadId: sub.TopicId);
                logger.LogInformation("New message to" + sub.Title);
            }

            logger.LogInformation("Message sent to subscribers.");
        }
    }

    protected override async Task WaitDelay()
    {
        // Генерируем случайный интервал от 1 до 1.5 часа (от 3600 до 5400 секунд)
        var interval = TimeSpan.FromSeconds(_random.Next(15, 30));
        logger.LogInformation($"Next message by {interval} seconds.");
        // Ждем интервал
        await Task.Delay(interval);
    }
}