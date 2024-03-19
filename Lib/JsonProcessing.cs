using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Lib.Entities;

namespace Lib;

public class JsonProcessing
{
    public bool State { get; private set; }
    public string? Path { get; private set; }
    
    private readonly JsonSerializerOptions options = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
    };
    
    public JsonProcessing() {}
    public JsonProcessing(string path) => Path = path;
    
    /// <summary>
    /// Reads filestream. Saves in user's data file. Returns collection.
    /// </summary>
    public async Task<List<NetPoint>> Read(FileStream stream)
    {
        var collection = new List<NetPoint>();
        try
        {
            collection = await JsonSerializer.DeserializeAsync<List<NetPoint>>(stream);
            State = true;
        }
        catch (Exception) { State = false; }
        
        return collection ?? new List<NetPoint>();
    }

    /// <summary>
    /// Remakes collection to stream to send to telegram.
    /// </summary>
    public async Task<Stream> Write(IEnumerable<NetPoint> points)
    {
        if (Path == null) return Stream.Null;
        
        var stream = File.Open(Path, FileMode.Create);
        try
        {
            await JsonSerializer.SerializeAsync(stream, points, options);
            State = true;
        }
        catch (Exception) { State = false; }
        
        return stream;
    }
}