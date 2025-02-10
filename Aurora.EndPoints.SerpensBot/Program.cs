using Aurora.EndPoints.SerpensBot.Repositories;
using Aurora.EndPoints.SerpensBot.Services.CommandService;
using Aurora.EndPoints.SerpensBot.Services.NotificationService;
using Aurora.EndPoints.SerpensBot.Services.TelegramBotService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zefirrat.YandexGpt.AspNet.Di;

namespace Aurora.EndPoints.SerpensBot;

class Program
{
    static async Task Main(string[] args)
    {
        var services = AddCoreServices();
        services.GetService<ITelegramService>()!.StartReceiving();
        // Создаем экземпляр сервиса
        var notificationService = services.GetService<NotificationService>()!;

        // Запускаем задачу в фоновом режиме
        var task = notificationService.StartAsync();

        // Ждём сигнал остановки от пользователя
        await Task.Run(() =>
        {
            while (true)
            {
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.C && Console.NumberLock == false)
                {
                    notificationService.Stop(); // Отправляем сигнал остановки
                    break;
                }
            }
        });

        await task;
    }

    private static IConfiguration AddConfiguration()
    => new ConfigurationManager()
        .AddJsonFile(Directory.GetCurrentDirectory() + @"..\..\..\..\settings.json")
        .Build();

    private static IServiceProvider AddCoreServices()
        => new ServiceCollection()
            .AddLogging()
            .AddSingleton<ISubscribersRepository, LocalSubscribersRepository>()
            .AddScoped<ICommandService, CommandService>()
            .AddSingleton<ITelegramService, TelegramService>()
            .AddScoped<NotificationService>()
            .AddYandexGpt(AddConfiguration())
            .BuildServiceProvider();
}