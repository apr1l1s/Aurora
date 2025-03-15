namespace Aurora.Domain.Core.Base;

public interface IEntity<out TId>
{
    public TId Id { get; }
}