using Aurora.EndPoints.SerpensBot.Repositories;
using Aurora.EndPoints.SerpensBot.Services;
using Aurora.EndPoints.SerpensBot.Services.CommandService;
using Aurora.EndPoints.SerpensBot.Services.NotificationService;
using Aurora.EndPoints.SerpensBot.Services.TelegramBotService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Zefirrat.YandexGpt.AspNet.Di;

namespace Aurora.EndPoints.SerpensBot;

class Program
{
    static async Task Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddHttpClient("telegram_bot_client").RemoveAllLoggers()
                    .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                    {
                        TelegramBotClientOptions options = new("7877107836:AAHMpLMEl_KfWog0jrx-qgKrw2jQFHkB6L8");
                        return new TelegramBotClient(options, httpClient);
                    });

                services
                    .AddSingleton<ISubscribersRepository, LocalSubscribersRepository>()
                    .AddScoped<ICommandService, CommandService>()
                    //.AddHostedService<NotificationService>()
                    .AddScoped<UpdateHandler>()
                    .AddScoped<ReceiverService>()
                    .AddHostedService<PollingService>()
                    .AddYandexGpt(AddConfiguration())
                    .BuildServiceProvider();
            }).Build();

        await host.RunAsync();
    }

    private static IConfiguration AddConfiguration()
    => new ConfigurationManager()
        .AddJsonFile(Directory.GetCurrentDirectory() + @"..\..\..\..\settings.json")
        .Build();
}