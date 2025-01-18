using Aurora.EndPoints.AlpheratzBot.Repositories;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Aurora.EndPoints.AlpheratzBot.Services;

public class RandomMessageScheduler(TelegramBotClient telegramBot,
    ISubscribersRepository subscribersRepository,
    ILogger<RandomMessageScheduler> logger)
: MessageScheduler
{
    private readonly Random _random = new();

    protected override async Task Schedule()
    {
        while (true)
        {
            var date = DateTime.Now;
            if (date.DayOfWeek is DayOfWeek.Sunday or DayOfWeek.Saturday)
            {
                await Task.Delay(3600*24);
            }
            if (date.Hour > 9 && date.Hour < 18)
            {
                await WaitDelay();
                await OnMessage();
            }
        }
    }

    protected override async Task OnMessage()
    {
        while (true)
        {
            await WaitDelay();
            var date = DateTime.Now;

            var message =
                "\ud83d\udea8 ОПОВЕЩЕНИЕ: КУРЕНИЕ.\n\n" +
                "\u26a0\ufe0f ВАЖНОЕ УВЕДОМЛЕНИЕ:\n\n" +
                $"\u23f3 Работы по курению у падика начинаются в {date.Hour}:{date.Minute + 5} и будут продолжаться в течение следующих нескольких минут.\n" +
                "Все операции с компьютером будут приостановлены до окончания курения.\n\n" +
                "\u23f0 Будьте уверены, что мы проинформируем вас, когда процесс работы будет вновь доступен. До получения отбойного сообщения \n\n" +
                "\ud83d\udeabРАБОТАТЬ НЕЛЬЗЯ!\ud83d\udeab\n\n" +
                "Спасибо за понимание и сотрудничество! \ud83d\ude4f";
            
            var subscribers = subscribersRepository.GetCollection();
            foreach (var sub in subscribers)
            {
                var file = new FileInfo(
                    "C:\\Users\\safronov.a\\RiderProjects\\Aurora\\Aurora.EndPoints.AlpheratzBot\\image.png");
                var stream = file.OpenRead();
                var image = InputFile.FromStream(stream);
                await telegramBot.SendPhoto(sub.ChannelId, image, message);
                logger.LogInformation("New message to" + sub.Title);
            }

            logger.LogInformation("Message sent to subscribers.");
        }
    }

    protected override async Task WaitDelay()
    {
        // Генерируем случайный интервал от 1 до 1.5 часа (от 3600 до 5400 секунд)
        var interval = TimeSpan.FromSeconds(_random.Next(3600, 5400));
        logger.LogInformation($"Next message by {interval} seconds.");
        // Ждем интервал
        await Task.Delay(interval);
    }
}