using GreenZoneWifiBot.Services;

namespace GreenZoneWifiBot.Abstractions;

public abstract class PollingServiceBase : BackgroundService
{
    private readonly ILogger<PollingServiceBase> _logger;
    private readonly IServiceProvider _serviceProvider;
    
    protected PollingServiceBase(IServiceProvider serviceProvider, ILogger<PollingServiceBase> logger)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }
 
    protected override async Task ExecuteAsync(CancellationToken cts)
    {
        _logger.LogInformation("Starting polling");
        
        while (!cts.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var receiver = scope.ServiceProvider.GetRequiredService<ReceiverService>();

                await receiver.ReceiveAsync(cts);
            }
            catch (Exception e)
            {
                _logger.LogError("Polling failed: {ErrorMessage}", e.Message);
                await Task.Delay(TimeSpan.FromSeconds(5), cts);
            }
        }
    }   
}