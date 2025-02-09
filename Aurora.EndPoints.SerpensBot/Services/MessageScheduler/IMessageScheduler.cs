namespace Aurora.EndPoints.SerpensBot.Services.MessageScheduler;

public interface IMessageScheduler
{
    public Task Schedule();

    public Task OnMessage();

    public Task WaitDelay();
}