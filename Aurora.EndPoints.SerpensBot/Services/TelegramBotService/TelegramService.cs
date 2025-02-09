using Aurora.EndPoints.SerpensBot.Services.CommandService;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using ISubscribersRepository = Aurora.EndPoints.SerpensBot.Repositories.ISubscribersRepository;

namespace Aurora.EndPoints.SerpensBot.Services.TelegramBotService;

public class TelegramService
    : ITelegramService
{
    private readonly TelegramBotClient _client;
    private readonly ICommandService _commandService;
    private readonly ISubscribersRepository _subscribersRepository;
    private readonly ILogger<TelegramService> _logger;

    public TelegramService(ICommandService commandService, ISubscribersRepository subscribersRepository,
        ILogger<TelegramService> logger)
    {
        _client = new TelegramBotClient("7877107836:AAHMpLMEl_KfWog0jrx-qgKrw2jQFHkB6L8");
        _commandService = commandService;
        _subscribersRepository = subscribersRepository;
        _logger = logger;

        StartReceiving();
    }

    public async Task SendMessageAsync(string message, CancellationToken cancellationToken)
    {
        foreach (var sub in _subscribersRepository.GetCollection())
        {
            // var file = new FileInfo(
            //     "C:\\Users\\safronov.a\\RiderProjects\\Aurora\\Aurora.EndPoints.AlpheratzBot\\image.png");
            // var stream = file.OpenRead();
            // var image = InputFile.FromStream(stream);
            // await _client.SendPhoto(sub.ChannelId, image, message, cancellationToken: cancellationToken);
            _logger.LogInformation("New message to " + sub.Title);
            await  _client.SendMessage(sub.ChannelId, message, messageThreadId: sub.TopicId);
        }
    }

    public void StartReceiving()
    {
        _client.StartReceiving(HandleUpdateAsync, HandleErrorAsync);
    }
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        await _commandService.HandleCommand(_client, update, cancellationToken).ConfigureAwait(false);
    }

    public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source,
        CancellationToken cancellationToken)
    {
        //logger.LogInformation($"Unknown error:\n{exception.Message}");
    }
}