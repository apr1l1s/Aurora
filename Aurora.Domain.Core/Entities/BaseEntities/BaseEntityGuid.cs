namespace Core.Entities;

public class BaseEntityGuid
    : BaseEntityT<Guid>
{
    public BaseEntityGuid()
        : base()
    {
        Id = Guid.NewGuid();
    }
}