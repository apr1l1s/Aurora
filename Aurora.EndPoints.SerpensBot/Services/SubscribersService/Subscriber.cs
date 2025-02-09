using Telegram.Bot.Types;

namespace Aurora.EndPoints.SerpensBot.Services.SubscribersService;

public record Subscriber(long ChannelId, string Title, int? TopicId)
{
    public Subscriber(Message message)
        : this(message.Chat.Id, message.Chat.Title, message.MessageThreadId)
    {
    }
}