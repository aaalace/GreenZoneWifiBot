using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

namespace Lib;

public class CsvProcessing
{
    public bool State { get; private set; }
    
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
                        if (headersCounter == 2) continue;
                        csv.ReadHeader();
                        continue;
                    }
                    
                    collection.Add(csv.GetRecord<NetPoint>());  
                }  
                catch (Exception e)
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
    
    // public FileStream Write(List<NetPoint> points)
    // {
    //     return new FileStream("");
    // }
}