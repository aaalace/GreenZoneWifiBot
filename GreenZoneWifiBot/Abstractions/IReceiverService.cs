namespace GreenZoneWifiBot.Abstractions;

public interface IReceiverService
{
    Task ReceiveAsync(CancellationToken cts);
}