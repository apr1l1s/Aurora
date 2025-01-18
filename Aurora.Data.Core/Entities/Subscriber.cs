using Telegram.Bot.Types;

namespace Aurora.Data.Core.Entities;

public record Subscriber(long ChannelId, string Title, int? TopicId = null)
{
    public Subscriber(Message message)
        : this(message.Chat.Id, message.Chat.Title, message.MessageThreadId)
    {
    }
}