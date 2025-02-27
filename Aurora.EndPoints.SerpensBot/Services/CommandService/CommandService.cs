using Aurora.EndPoints.SerpensBot.Helpers;
using Aurora.EndPoints.SerpensBot.Services.SubscribersService;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Zefirrat.YandexGpt.Prompter;
using ISubscribersRepository = Aurora.EndPoints.SerpensBot.Repositories.ISubscribersRepository;

namespace Aurora.EndPoints.SerpensBot.Services.CommandService;


public class CommandService(ISubscribersRepository subscribersRepository,
    YaPrompter prompter,
    ILogger<CommandService> logger)
    : ICommandService
{
    public async Task HandleCommand(TelegramBotClient telegramBot, Update update, CancellationToken cancellationToken)
    {
        var message = update.Message;
        if (message == null)
        {
            return;
        }

        var sub = new Subscriber(message);
        var command = message.Text;

        switch (command)
        {
            case "/sub":
                if (subscribersRepository.AddItem(sub))
                {
                    await telegramBot.SendMessage(message.Chat.Id, "Лайк, подписка",
                        messageThreadId: message.MessageThreadId, cancellationToken: cancellationToken);
                    logger.LogInformation($"New subscriber: {message.Chat.Id}" + (message.MessageThreadId.HasValue ? message.MessageThreadId.Value.ToString() : ""));
                }
                break;

            case "/unsub":
                //await subscribersService.TryUnsub(sub, message, cancellationToken);
                if (subscribersRepository.RemoveItem(message.Chat.Id) > 0)
                {
                    await telegramBot.SendMessage(message.Chat.Id, "Понял, вычеркиваю.",
                        messageThreadId: message.MessageThreadId, cancellationToken: cancellationToken);
                    logger.LogInformation($"Unsubscribed: {message.Chat.Id}");
                }
                break;

            case "/list":
                var subs = subscribersRepository.GetCollection().Select(sub => sub.Title);
                string response = "";
                foreach (var subscriber in subs)
                {
                    response += subscriber + Environment.NewLine;
                }

                await telegramBot.SendMessage(message.Chat.Id, response, messageThreadId: message.MessageThreadId,
                    cancellationToken: cancellationToken);
                break;

            case "/alert":
                await SendTelegramNotification(telegramBot,message.Chat.Id, message.MessageThreadId);
                break;

            default:
                if (command != null)
                {
                    //await telegramBot.SendMessage(message.Chat.Id, command, cancellationToken: cancellationToken);
                }

                logger.LogInformation($"Unknown message: {message?.Text}");

                break;
        }
    }

    private async Task SendTelegramNotification(TelegramBotClient telegramBot, ChatId chatId, int? topicId)
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
        await telegramBot.SendMessage(chatId, response ?? "Алиса отдыхает, ебальник завали и иди курить", messageThreadId:topicId);
    }
}