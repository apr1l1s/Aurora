using Telegram.Bot;
using Telegram.Bot.Types;

namespace Aurora.EndPoints.SerpensBot.Services.CommandService;

public interface ICommandService
{
    Task HandleCommand(TelegramBotClient telegramBot, Update update, CancellationToken cancellationToken);
}