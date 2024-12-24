using Aurora.EndPoints.AlpheratzBot.Services;

namespace Aurora.EndPoints.AlpheratzBot.Repositories;

public interface ISubscribersRepository
{
    public List<Subscriber> GetCollection();

    public Subscriber? GetItemNullable(long channelId);
    
    public bool AddItem(Subscriber subscriber);

    public int RemoveItem(long channelId);
}

public class LocalSubscribersRepository : ISubscribersRepository
{
    private readonly HashSet<Subscriber> _subscribers = [];

    public List<Subscriber> GetCollection() => _subscribers.ToList();

    public Subscriber? GetItemNullable(long channelId) =>
        _subscribers.FirstOrDefault(sub => sub.ChannelId == channelId);

    public bool AddItem(Subscriber subscriber) => _subscribers.Add(subscriber);

    public int RemoveItem(long channelId) => _subscribers.RemoveWhere(x => x.ChannelId == channelId);
}