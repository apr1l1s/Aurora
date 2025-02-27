using Aurora.EndPoints.SerpensBot.Services;
using Aurora.EndPoints.SerpensBot.Services.BaseServices;
using Aurora.EndPoints.SerpensBot.Services.Handlers;
using Aurora.EndPoints.SerpensBot.Services.HostedServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Zefirrat.YandexGpt.AspNet.Di;

namespace Aurora.EndPoints.SerpensBot;

internal class Program
{
    static async Task Main(string[] args)
        => await Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                var tToken = context.Configuration.GetSection("TelegramBotOptions:Token").Get<string>();

                services.AddHttpClient("telegram_bot_client").RemoveAllLoggers()
                    .AddTypedClient<ITelegramBotClient>((httpClient, _) =>
                        new TelegramBotClient(new TelegramBotClientOptions(tToken!), httpClient));

                services.AddScoped<UpdateHandler>()
                    .AddScoped<ReceiverService>()
                    .AddHostedService<PollingService>()
                    .AddYandexGpt(context.Configuration)
                    .BuildServiceProvider();
            })
            .ConfigureLogging(logging => logging.ClearProviders().AddConsole().AddDebug()).Build()
            .RunAsync();
}