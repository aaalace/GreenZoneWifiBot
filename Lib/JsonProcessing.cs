using System.Text.Json;

namespace Lib;

public class JsonProcessing
{
    public bool State { get; private set; }
    
    public async Task<List<NetPoint>> Read(FileStream stream)
    {
        var collection = new List<NetPoint>();

        try
        {
            collection = await JsonSerializer.DeserializeAsync<List<NetPoint>>(stream);
            State = true;
        }
        catch (Exception)
        {
            State = false;
        }
        
        return collection ?? new List<NetPoint>();
    }
    
    // public static FileStream Write(List<NetPoint> points)
    // {
    //     return new FileStream("");
    // }
}