namespace Aurora.EndPoints.BorealisBot.Services;

public abstract class MessageScheduler
{
    public virtual void Start()
    {
        Task.Run(OnMessage);
    }

    protected abstract Task OnMessage();

    protected abstract Task WaitDelay();
}