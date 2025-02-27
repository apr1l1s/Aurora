using Aurora.EndPoints.SerpensBot.Services.BaseServices;
using Aurora.EndPoints.SerpensBot.Services.Handlers;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace Aurora.EndPoints.SerpensBot.Services.HostedServices;

public class ReceiverService(ITelegramBotClient botClient, UpdateHandler updateHandler, ILogger<ReceiverServiceBase<UpdateHandler>> logger)
    : ReceiverServiceBase<UpdateHandler>(botClient, updateHandler, logger);