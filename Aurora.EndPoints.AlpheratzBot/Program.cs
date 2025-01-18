using Aurora.EndPoints.AlpheratzBot.Repositories;
using Aurora.EndPoints.AlpheratzBot.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

const string token = "7877107836:AAHMpLMEl_KfWog0jrx-qgKrw2jQFHkB6L8";

var serviceProvider = new ServiceCollection()
    .AddLogging(builder =>  builder.AddConsole())
    .AddSingleton(new TelegramBotClient(token))
    .AddSingleton<ITelegramBotService, TelegramBotService>()
    .AddSingleton<MessageScheduler, RandomMessageScheduler>()
    .AddSingleton<ISubscribersRepository, LocalSubscribersRepository>()
    .BuildServiceProvider();

var botService = serviceProvider.GetService<ITelegramBotService>();
if (botService == null)
{
    throw new ApplicationException("Неудачная попытка подключения телеграм бота");
}
botService.StartBotAsync(token);

var messageScheduler = serviceProvider.GetService<MessageScheduler>();
if (messageScheduler == null)
{
    throw new ApplicationException("Неудачная попытка создания расписания");
}

messageScheduler.Start();
Console.ReadLine();