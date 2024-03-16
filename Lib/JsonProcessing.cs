using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Lib.Entities;

namespace Lib;

public class JsonProcessing
{
    public bool State { get; private set; }
    
    private readonly JsonSerializerOptions options = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
    };
    
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

    public async Task<FileStream> Write(List<NetPoint> points, string path)
    {
        var stream = File.Open(path, FileMode.Create);

        try
        {
            await JsonSerializer.SerializeAsync(stream, points, options);
            State = true;
        }
        catch (Exception)
        {
            State = false;
        }
        
        return stream;
    }
    
    public Task<Stream> Write(List<NetPoint> points)
    {
        Stream stream = new MemoryStream();
        
        try
        {
            string str = JsonSerializer.Serialize(points, options);
            stream = GenerateStreamFromString(str);
            State = true;
        }
        catch (Exception)
        {
            State = false;
        }
        stream.Close();
        
        return Task.FromResult(stream);
    }
    
    private static Stream GenerateStreamFromString(string s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }
}