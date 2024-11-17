namespace Core.Entities;

public class BaseEntity
{
    public DateTime DateAdded { get; set; }
    public DateTime DateUpdated { get; set; }

    public BaseEntity()
    {
        DateAdded = DateTime.Now;
        DateUpdated = DateTime.Now;
    }

    public void Update()
    {
        DateUpdated = DateTime.Now;
    }
}