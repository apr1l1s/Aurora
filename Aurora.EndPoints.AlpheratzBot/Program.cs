using Aurora.EndPoints.BorealisBot.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

// Указываем токен для бота
const string token = "7877107836:AAHMpLMEl_KfWog0jrx-qgKrw2jQFHkB6L8";

var serviceProvider = new ServiceCollection()
    .AddLogging(builder =>  builder.AddConsole())
    .AddSingleton(new TelegramBotClient(token))
    .AddSingleton<ITelegramBotService, TelegramBotService>()
    .AddSingleton<MessageScheduler, RandomMessageScheduler>()
    .AddSingleton<ISubscribersRepository, LocalSubscribersRepository>()
    .BuildServiceProvider();

// Инициализируем бота
var botService = serviceProvider.GetService<ITelegramBotService>();
if (botService == null)
{
    throw new ApplicationException("Неудачная попытка подключения телеграм бота");
}
botService.StartBotAsync(token);

// Запускаем сервис планирования сообщений
var messageScheduler = serviceProvider.GetService<MessageScheduler>();
if (messageScheduler == null)
{
    throw new ApplicationException("Неудачная попытка создания расписания");
}

messageScheduler.Start();
Console.ReadLine();