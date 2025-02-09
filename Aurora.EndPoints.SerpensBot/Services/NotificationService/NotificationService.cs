using System.Text;
using Aurora.EndPoints.SerpensBot.Helpers;
using Aurora.EndPoints.SerpensBot.Services.TelegramBotService;
using Zefirrat.YandexGpt.Abstractions;
using Zefirrat.YandexGpt.Prompter;

namespace Aurora.EndPoints.SerpensBot.Services.NotificationService;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

public class NotificationService(ILogger<NotificationService> logger, 
    ITelegramService telegramService, 
    IYaPrompter prompter) 
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public async Task StartAsync()
    {
        try
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                // Проверяем, является ли сегодня рабочим днем
                if (!CalendarHelper.IsWorkingDay(DateTime.Now.Date))
                {
                    logger.LogInformation("Сегодня не рабочий день. Задача пропущена.");
                    await Task.Delay(TimeSpan.FromHours(24), _cancellationTokenSource.Token);
                    continue;
                }

                // Рандомизация времени от 1 до 1,5 часов
                var random = new Random();
                var delayMinutes = random.Next(10, 20); // От 60 до 90 минут
                logger.LogInformation($"Следующая отправка через {delayMinutes} минут.");

                // Ждём заданное время
                await Task.Delay(TimeSpan.FromSeconds(delayMinutes), _cancellationTokenSource.Token);

                // Отправляем уведомление
                await SendTelegramNotification();
                logger.LogInformation("Уведомление отправлено.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при выполнении задачи.");
        }
    }

    public void Stop()
    {
        _cancellationTokenSource.Cancel();
    }

    private async Task SendTelegramNotification()
    {
        // Ваш код для отправки Telegram-сообщения
        Console.WriteLine("Отправка уведомления...");
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
                sb.AppendLine("Так же можешь туда написать случайную шутку. ");
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

        string? response = null;
        try
        {
            response = await prompter.SendAsync(sb.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        Console.WriteLine(response ?? "Пусто");
        await telegramService.SendMessageAsync(response ?? "Алиса отдыхает, ебальник завали и иди курить");
    }
}