using Aurora.Domain.Core.Base;

namespace Aurora.EndPoints.SerpensBot.Repositories;

public interface IRepository<TEntity, in TId> 
    where TEntity : IEntity<TId>
{
    Task<TEntity?> GetNullableItemAsync(TId id);
    
    Task<TEntity> GetItemAsync(TId id);
    
    Task<TEntity> GetOrCreate(TId id);

    Task<IReadOnlyList<TEntity>> GetCollectionAsync(IReadOnlyList<TId> ids);

    Task<bool> DeleteAsync(TId id);

    Task<bool> SaveAsync();
}