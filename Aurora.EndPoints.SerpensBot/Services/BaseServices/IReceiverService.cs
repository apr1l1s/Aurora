namespace Aurora.EndPoints.SerpensBot.Services.BaseServices;

public interface IReceiverService
{
    Task ReceiveAsync(CancellationToken stoppingToken);
}