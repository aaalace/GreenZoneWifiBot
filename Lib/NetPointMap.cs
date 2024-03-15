using CsvHelper.Configuration;

namespace Lib;

sealed class NetPointMap : ClassMap<NetPoint>
{
    public NetPointMap()
    {
        Map(m => m.Id).Name("ID");
        Map(m => m.GlobalId).Name("global_id");
        Map(m => m.Name).Name("Name");
        Map(m => m.AdmArea).Name("AdmArea");
        Map(m => m.District).Name("District");
        Map(m => m.ParkName).Name("ParkName");
        Map(m => m.WifiName).Name("WiFiName");
        Map(m => m.CoverageArea).Name("CoverageArea");
        Map(m => m.FunctionFlag).Name("FunctionFlag");
        Map(m => m.AccessFlag).Name("AccessFlag");
        Map(m => m.Password).Name("Password");
        Map(m => m.Longitude).Name("Longitude_WGS84");
        Map(m => m.Latitude).Name("Latitude_WGS84");
        Map(m => m.GeodataCentre).Name("geodata_center");
        Map(m => m.GeoArea).Name("geoarea");
    }
}