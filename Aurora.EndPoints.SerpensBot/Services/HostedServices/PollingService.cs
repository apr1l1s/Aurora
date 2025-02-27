using Aurora.EndPoints.SerpensBot.Services.BaseServices;
using Microsoft.Extensions.Logging;

namespace Aurora.EndPoints.SerpensBot.Services.HostedServices;

public class PollingService(IServiceProvider serviceProvider, ILogger<PollingService> logger)
    : PollingServiceBase<ReceiverService>(serviceProvider, logger);