using System.Text;
using Aurora.EndPoints.SerpensBot.Helpers;
using Aurora.EndPoints.SerpensBot.Services.TelegramBotService;
using Microsoft.Extensions.Hosting;
using Zefirrat.YandexGpt.Abstractions;
using Microsoft.Extensions.Logging;

namespace Aurora.EndPoints.SerpensBot.Services.NotificationService;


public class NotificationService(ILogger<NotificationService> logger, 
    ITelegramService telegramService, 
    IYaPrompter prompter) 
:IHostedService
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                // Проверяем, является ли сегодня рабочим днем
                if (CalendarHelper.IsWorkingDay(DateTime.Now))
                {
                    logger.LogInformation("Сегодня не рабочий день. Задача пропущена.");
                    await Task.Delay(TimeSpan.FromHours(1), _cancellationTokenSource.Token);
                    continue;
                }

                // Отправляем уведомление
                await SendTelegramNotification();

                // Рандомизация времени от 1 до 1,5 часов
                var random = new Random();
                var delayMinutes = random.Next(60, 90); // От 60 до 90 минут
                logger.LogInformation($"Следующая отправка через {delayMinutes} минут.");

                // Ждём заданное время
                await Task.Delay(TimeSpan.FromMinutes(delayMinutes), _cancellationTokenSource.Token);

                logger.LogInformation("Уведомление отправлено.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при выполнении задачи.");
        }
    }

    private async Task SendTelegramNotification()
    {
        // Ваш код для отправки Telegram-сообщения
        logger.LogInformation("Отправка уведомления...");

        string? response = null;
        try
        {
            response = await prompter.SendAsync(PromptHelper.GetRandomPrompt());
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex.Message);
        }
        logger.LogInformation(response ?? "Пусто");
        await telegramService.SendMessageAsync(response ?? "Алиса отдыхает, ебальник завали и иди курить");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }
}