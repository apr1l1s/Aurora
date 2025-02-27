using Aurora.EndPoints.SerpensBot.Helpers;
using Aurora.EndPoints.SerpensBot.Services.CommandService;
using Aurora.EndPoints.SerpensBot.Services.SubscribersService;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Zefirrat.YandexGpt.Prompter;
using ISubscribersRepository = Aurora.EndPoints.SerpensBot.Repositories.ISubscribersRepository;

namespace Aurora.EndPoints.SerpensBot.Services.TelegramBotService;

public class TelegramService
    : ITelegramService
{
    private readonly TelegramBotClient _client;
    private readonly ISubscribersRepository _subscribersRepository;
    private readonly ILogger<TelegramService> _logger;
    private readonly YaPrompter _prompter;

    public TelegramService(ISubscribersRepository subscribersRepository, YaPrompter prompter,
        ILogger<TelegramService> logger)
    {
        _client = new TelegramBotClient("7877107836:AAHMpLMEl_KfWog0jrx-qgKrw2jQFHkB6L8");
        _subscribersRepository = subscribersRepository;
        _prompter = prompter;
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
                if (_subscribersRepository.AddItem(sub))
                {
                    await botClient.SendMessage(message.Chat.Id, "Лайк, подписка",
                        messageThreadId: message.MessageThreadId, cancellationToken: cancellationToken);
                    _logger.LogInformation($"New subscriber: {message.Chat.Id}" + (message.MessageThreadId.HasValue ? message.MessageThreadId.Value.ToString() : ""));
                }
                break;

            case "/unsub":
                //await subscribersService.TryUnsub(sub, message, cancellationToken);
                if (_subscribersRepository.RemoveItem(message.Chat.Id) > 0)
                {
                    await botClient.SendMessage(message.Chat.Id, "Понял, вычеркиваю.",
                        messageThreadId: message.MessageThreadId, cancellationToken: cancellationToken);
                    _logger.LogInformation($"Unsubscribed: {message.Chat.Id}");
                }
                break;

            case "/list":
                var subs = _subscribersRepository.GetCollection().Select(sub => sub.Title);
                string response = "";
                foreach (var subscriber in subs)
                {
                    response += subscriber + Environment.NewLine;
                }

                await botClient.SendMessage(message.Chat.Id, response, messageThreadId: message.MessageThreadId,
                    cancellationToken: cancellationToken);
                break;

            case "/alert":
                await SendTelegramNotification(message.Chat.Id, message.MessageThreadId);
                break;

            default:
                if (command != null)
                {
                    //await telegramBot.SendMessage(message.Chat.Id, command, cancellationToken: cancellationToken);
                }

                _logger.LogInformation($"Unknown message: {message?.Text}");

                break;
        }
    }

    private async Task SendTelegramNotification(ChatId chatId, int? topicId)
    {
        // Ваш код для отправки Telegram-сообщения
        _logger.LogInformation("Отправка уведомления...");

        string? response = null;
        try
        {
            response = await _prompter.SendAsync(PromptHelper.GetRandomPrompt());
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex.Message);
        }
        _logger.LogInformation(response ?? "Пусто");
        await _client.SendMessage(chatId, response ?? "Алиса отдыхает, ебальник завали и иди курить", messageThreadId:topicId);
    }

    public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source,
        CancellationToken cancellationToken)
    {
        //logger.LogInformation($"Unknown error:\n{exception.Message}");
    }
}