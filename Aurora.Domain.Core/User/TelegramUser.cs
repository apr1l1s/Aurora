using Aurora.Domain.Core.Base;

namespace Aurora.Domain.Core.User;

public class TelegramUser : IEntity<long>
{
    public long Id { get; set; }

    public int Status { get; set; }

    public void UpdateStatus(int value, bool status)
    {
        Status += status ? value : -value;
    }
}