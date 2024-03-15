namespace GreenZoneWifiBot.Utils.Logging;

public class FileLogger : ILogger
{
    private readonly string _path;
    private static readonly object _lock = new();
    
    public FileLogger(string path) => _path = path;
    
    public IDisposable? BeginScope<TState>(TState state) => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception, string>? formatter)
    {
        lock (_lock)
        {
            string fullFilePath = Path.Combine(_path, DateTime.Now.ToString("yyyy-MM-dd") + "_log.txt");
            File.AppendAllText(fullFilePath, $"{state}{Environment.NewLine}");
        }
    }
}