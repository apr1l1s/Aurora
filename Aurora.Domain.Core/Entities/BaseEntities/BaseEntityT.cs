namespace Core.Entities;

public class BaseEntityT<T>
    : BaseEntity
{
    public T Id { get; set; }
}