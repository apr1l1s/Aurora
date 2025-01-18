using Aurora.EndPoints.AlpheratzBot.Repositories;
using Aurora.Data.Core.Entities;
using Core.Providers.Cqrs;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Aurora.EndPoints.BorealisBot.Services;

public record CommandUseCase(string? Command, Message Message)
    : IUseCase;

public class CommandHandler(ISubscribersRepository subscribersRepository,
    TelegramBotClient telegramBotClient,
    ILogger<CommandHandler> logger)
:IUseCaseHandler<CommandUseCase>
{
    public async Task Handle(CommandUseCase request, CancellationToken cancellationToken)
    {
        switch (request.Command)
        {
            case "/sub":
                var sub = new Subscriber(request.Message);
                if (subscribersRepository.AddItem(sub))
                {
                    await telegramBotClient.SendMessage(request.Message.Chat.Id, "Лайк, подписка",
                        messageThreadId: request.Message.MessageThreadId, cancellationToken: cancellationToken);
                    logger.LogInformation($"new_sub_id: {request.Message.Chat.Id}");
                }

                break;

            case "/unsub":
                if (subscribersRepository.RemoveItem(request.Message.Chat.Id) > 0)
                {
                    await telegramBotClient.SendMessage(request.Message.Chat.Id, "Понял, вычеркиваю.",
                        cancellationToken: cancellationToken);
                    logger.LogInformation($"unsub_id: {request.Message.Chat.Id}");
                }

                break;

            default:
                if (request.Message != null)
                {
                    await telegramBotClient.SendMessage(request.Message.Chat.Id, request.Command, cancellationToken: cancellationToken);
                }
                logger.LogInformation($"unk: {request.Message.Text}");

                break;
        }
    }
}