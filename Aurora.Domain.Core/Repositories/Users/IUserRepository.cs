using Aurora.Domain.Core.User;
using Aurora.EndPoints.SerpensBot.Repositories;

namespace Aurora.Domain.Core.Repositories.Users;

public interface IUserRepository : IRepository<TelegramUser, long>;