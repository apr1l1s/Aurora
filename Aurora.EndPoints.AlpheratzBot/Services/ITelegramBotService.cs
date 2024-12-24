using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Aurora.EndPoints.AlpheratzBot.Services;

public interface ITelegramBotService
{
    void StartBotAsync(string token);

    Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);

    Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, 
        CancellationToken cancellationToken);
}