using GreenZoneWifiBot.Abstractions;

namespace GreenZoneWifiBot.Services;

public class PollingService : PollingServiceBase
{
    public PollingService(IServiceProvider serviceProvider, ILogger<PollingService> logger)
        : base(serviceProvider, logger) {}
}