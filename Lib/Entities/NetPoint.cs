using System.Text.Json.Serialization;

namespace Lib.Entities;

public class NetPoint
{
    [JsonPropertyName("ID")]
    public string Id { get; set; } = null!;
    [JsonPropertyName("global_id")]
    public string GlobalId { get; set; } = null!;
    [JsonPropertyName("Name")]
    public string Name { get; set; } = null!;
    [JsonPropertyName("AdmArea")]
    public string AdmArea { get; set; } = null!;
    [JsonPropertyName("District")]
    public string District { get; set; } = null!;
    [JsonPropertyName("ParkName")]
    public string ParkName { get; set; } = null!;
    [JsonPropertyName("WiFiName")]
    public string WifiName { get; set; } = null!;
    [JsonPropertyName("CoverageArea")]
    public string CoverageArea { get; set; } = null!;
    [JsonPropertyName("FunctionFlag")]
    public string FunctionFlag { get; set; } = null!;
    [JsonPropertyName("AccessFlag")]
    public string AccessFlag { get; set; } = null!;
    [JsonPropertyName("Password")]
    public string Password { get; set; } = null!;
    [JsonPropertyName("Longitude_WGS84")]
    public string Longitude { get; set; } = null!;
    [JsonPropertyName("Latitude_WGS84")]
    public string Latitude { get; set; } = null!;
    [JsonPropertyName("geodata_center")]
    public string GeodataCentre { get; set; } = null!;
    [JsonPropertyName("geoarea")]
    public string GeoArea { get; set; } = null!;

    public NetPoint() {}
    
    public NetPoint(
        string id,
        string globalId,
        string name,
        string admArea,
        string district,
        string parkName,
        string wifiName,
        string coverageArea,
        string functionFlag,
        string accessFlag,
        string password,
        string longitude,
        string latitude,
        string geodataCentre,
        string geoArea)
    {
        Id = id;
        GlobalId = globalId;
        Name = name;
        AdmArea = admArea;
        District = district;
        ParkName = parkName;
        WifiName = wifiName;
        CoverageArea = coverageArea;
        FunctionFlag = functionFlag;
        AccessFlag = accessFlag;
        Password = password;
        Longitude = longitude;
        Latitude = latitude;
        GeodataCentre = geodataCentre;
        GeoArea = geoArea;
    }
}