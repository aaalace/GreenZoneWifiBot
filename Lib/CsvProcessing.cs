using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using Lib.Entities;

namespace Lib;

public class CsvProcessing
{
    public bool State { get; private set; }
    // public string? RuHeader { get; private set; }
    
    private readonly CsvConfiguration configuration = new(CultureInfo.InvariantCulture)  
    {  
        Encoding = Encoding.UTF8,  
        Delimiter = ";"
    }; 

    public Task<List<NetPoint>> Read(FileStream stream)
    {
        var collection = new List<NetPoint>();
        
        try
        {
            var reader = new StreamReader(stream);
            var csv = new CsvReader(reader, configuration);
            csv.Context.RegisterClassMap<NetPointMap>();

            var headersCounter = 0;
            while (csv.Read())  
            {  
                try  
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
                catch (Exception)
                {
                    State = false;
                    return Task.FromResult(collection);
                }  
            }  
            
            reader.Close();
            csv.Dispose();
            State = true;
        }
        catch (Exception) { State = false; }

        return Task.FromResult(collection);
    }
    
    public FileStream Write(List<NetPoint> points, string path)
    {
        try
        {
            var writer = new StreamWriter(path);
            var csv = new CsvWriter(writer, configuration);
            csv.Context.RegisterClassMap<NetPointMap>();
            csv.WriteRecords(points);
            writer.Close();
                
            State = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            State = false;
        }

        var stream = File.Open(path, FileMode.OpenOrCreate);
        return stream;
    }
}