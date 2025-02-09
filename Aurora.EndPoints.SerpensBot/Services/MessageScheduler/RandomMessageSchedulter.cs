using System.Text;
using Aurora.EndPoints.SerpensBot.Services.TelegramBotService;
using Microsoft.Extensions.Logging;
using Zefirrat.YandexGpt.Prompter;

namespace Aurora.EndPoints.SerpensBot.Services.MessageScheduler;

public class RandomMessageScheduler(YaPrompter prompter,
    ITelegramService telegramService,
    ILogger<RandomMessageScheduler> logger)
    : IMessageScheduler
{
    private readonly Random _random = new();

    public async Task Schedule()
    {
        while (true)
        {
            var date = DateTime.Now;
            if (date.DayOfWeek is DayOfWeek.Sunday or DayOfWeek.Saturday)
            {
                await Task.Delay(3600 * 24);
            }

            if (date.Hour is > 9 and < 18)
            {
                await Task.Delay(TimeSpan.FromSeconds(10));
                await OnMessage();
                await WaitDelay();
            }
        }
    }

    public async Task OnMessage()
    {
        var date = DateTime.Now;

        string message =
            "\ud83d\udea8 ОПОВЕЩЕНИЕ: КУРЕНИЕ.\n\n" +
            "\u26a0\ufe0f ВАЖНОЕ УВЕДОМЛЕНИЕ:\n\n" +
            $"\u23f3 Работы по курению у падика начинаются в {date.Hour}:{date.Minute + 5} и будут продолжаться в течение следующих нескольких минут.\n" +
            "Все операции с компьютером будут приостановлены до окончания курения.\n\n" +
            "\u23f0 Будьте уверены, что мы проинформируем вас, когда процесс работы будет вновь доступен. До получения отбойного сообщения \n\n" +
            "\ud83d\udeabРАБОТАТЬ НЕЛЬЗЯ!\ud83d\udeab\n\n" +
            "Спасибо за понимание и сотрудничество! \ud83d\ude4f";

        message = await GetUniqueAlert() ?? message;

        await telegramService.SendMessageAsync(message, CancellationToken.None);
        
        logger.LogInformation("Message sent to subscribers.");
    }

    public async Task WaitDelay()
    {
        // Генерируем случайный интервал от 1 до 1.5 часа (от 3600 до 5400 секунд)
        var interval = TimeSpan.FromSeconds(_random.Next(3600, 5400));
        logger.LogInformation($"Next message by {interval} seconds.");
        // Ждем интервал
        await Task.Delay(interval);
    }

    private async Task<string?> GetUniqueAlert()
    {
        var rand = new Random();
        const int min = 1000;
        const int max = 3000;
        
        var sb = new StringBuilder();
        
        sb.AppendLine("Можешь написать текст, " +
                      "который будет использован для уведомления о том, что мы с коллегами пойдем курить. ");
        sb.AppendLine($"Текст должен быть длинной где то {rand.Next(min, max)} символов и содержать много смайликов. ");
        
        switch (rand.Next(min, max))
        {
            case < 1500:
                sb.AppendLine("Так же можешь туда поместить пару слов про то, какая погода на улице. ");
                break;
            case < 2000:
                sb.AppendLine("Так же можешь туда написать случайную шутку про украинцев. ");
                break;
            case < 2500:
                sb.AppendLine("Так же можешь упомянуть, несколько плюсов перерыва для работы. ");
                break;
            case < 3000:
                sb.AppendLine(
                    "Так же после сообщения добавь загадку и оставь ответ в виде скрытого текста. ");
                break;
        }
        
        sb.AppendLine(
            "Текст должен быть красив, уникален и смысл в том, что надо покурить, проветрится всем участником беседы.");

        var response = await prompter.SendAsync(sb.ToString());
        return response;
    }
}