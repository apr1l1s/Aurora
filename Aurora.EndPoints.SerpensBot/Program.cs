using Aurora.Domain.Core.Context;
using Aurora.EndPoints.SerpensBot.Services.Handlers;
using Aurora.EndPoints.SerpensBot.Services.HostedServices;
using Microsoft.EntityFrameworkCore;
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
    {
        Console.WriteLine("Welcome to Aurora - SerpensBot");
        await Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, configuration) =>
            {
                var appPath = string.Empty;
                var basePath = string.Empty;

                var environment = hostingContext.HostingEnvironment;
                if (environment.IsDevelopment())
                {
                    appPath = environment.ContentRootPath;
                }

                configuration.AddJsonFile(Path.Combine(appPath, $"appsettings.{environment.EnvironmentName}.json"),
                        optional: true, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                var tToken = context.Configuration.GetSection("TelegramBotOptions:Token").Get<string>();

                services.AddHttpClient("telegram_bot_client").RemoveAllLoggers()
                    .AddTypedClient<ITelegramBotClient>((httpClient, _) =>
                        new TelegramBotClient(new TelegramBotClientOptions(tToken!), httpClient));

                services
                    .AddScoped<UpdateHandler>()
                    .AddScoped<ReceiverService>()
                    .AddHostedService<PollingService>()
                    .AddDbContext<AppDbContext>()
                    .ConfigureDbContext<AppDbContext>(options => options.UseNpgsql(context.HostingEnvironment.IsStaging()
                        ? "Host=localhost:5432;Database=mydatabase;Username=user;Password=password"
                        : "Host=postgres;Database=mydatabase;Username=user;Password=password"))
                    .AddYandexGpt(context.Configuration)
                    .BuildServiceProvider();
            })
            .ConfigureLogging(logging => logging.ClearProviders().AddConsole().AddDebug()).Build()
            .RunAsync();
    }
}