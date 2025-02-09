using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Aurora.EndPoints.SerpensBot.Services.TelegramBotService;

public interface ITelegramService
{
    void StartReceiving();

    public Task SendMessageAsync(string message, CancellationToken cancellationToken = default);

    Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);

    Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, 
        CancellationToken cancellationToken);
}