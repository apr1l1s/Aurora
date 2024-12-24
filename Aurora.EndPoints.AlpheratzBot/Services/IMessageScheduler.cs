namespace Aurora.EndPoints.AlpheratzBot.Services;

public abstract class MessageScheduler
{
    public virtual void Start()
    {
        Task.Run(Schedule);
    }

    protected abstract Task Schedule();

    protected abstract Task OnMessage();

    protected abstract Task WaitDelay();
}