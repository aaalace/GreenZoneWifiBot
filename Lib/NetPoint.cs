namespace Lib;

public class NetPoint
{
    public string Id { get; set; } = null!;
    public string GlobalId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string AdmArea { get; set; } = null!;
    public string District { get; set; } = null!;
    public string ParkName { get; set; } = null!;
    public string WifiName { get; set; } = null!;
    public string CoverageArea { get; set; } = null!;
    public string FunctionFlag { get; set; } = null!;
    public string AccessFlag { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Longitude { get; set; } = null!;
    public string Latitude { get; set; } = null!;
    public string GeodataCentre { get; set; } = null!;
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