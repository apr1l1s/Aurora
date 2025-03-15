using Aurora.Domain.Core.User;

namespace Aurora.Domain.Core.Repositories.Users;

public class TelegramUserRepository
    : IUserRepository
{
    public Task<TelegramUser?> GetNullableItemAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<TelegramUser> GetItemAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<TelegramUser> GetOrCreate(long id)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<TelegramUser>> GetCollectionAsync(IReadOnlyList<long> ids)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SaveAsync()
    {
        throw new NotImplementedException();
    }
}