using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using Lib.Entities;

namespace Lib;

public class CsvProcessing
{
    public bool State { get; private set; }
    public string? Path { get; private set; }

    private readonly CsvConfiguration configuration = new(CultureInfo.InvariantCulture)  
    {  
        Encoding = Encoding.UTF8,  
        Delimiter = ";"
    }; 
    
    public CsvProcessing() {}
    public CsvProcessing(string path) => Path = path;

    /// <summary>
    /// Reads filestream. Saves in user's data file. Returns collection.
    /// </summary>
    public List<NetPoint> Read(FileStream stream)
    {
        var collection = new List<NetPoint>();
        
        try
        {
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, configuration);
            csv.Context.RegisterClassMap<NetPointMap>();

            var headersCounter = 0;
            while (csv.Read())
            {
                if (headersCounter < 2)
                {
                    headersCounter++;
                    if (headersCounter == 2)
                    {
                        // RuHeader = reader.ReadLine();
                        // if (RuHeader == null) throw new Exception("RuHeader do not exists");
                        continue;
                    }

                    csv.ReadHeader();
                    continue;
                }

                collection.Add(csv.GetRecord<NetPoint>());
            }  
            
            State = true;
        }
        catch (Exception) { State = false; }

        return collection;
    }
    
    /// <summary>
    /// Remakes collection to stream to send to telegram.
    /// </summary>
    public Stream Write(IEnumerable<NetPoint> points)
    {
        if (Path == null) { return Stream.Null;}
        
        try
        {
            using var writer = new StreamWriter(Path);
            using var csv = new CsvWriter(writer, configuration);
            csv.Context.RegisterClassMap<NetPointMap>();
            
            csv.WriteRecords(points);
            State = true;
        }
        catch (Exception) { State = false; }

        var stream = File.Open(Path, FileMode.OpenOrCreate);
        return stream;
    }
}