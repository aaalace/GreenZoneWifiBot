namespace GreenZoneWifiBot.Core;

public static class Fields
{
    public static readonly List<string> values = new() { "ID", "global_id", "Name",
        "AdmArea", "District", "ParkName", "WifiName", "CoverageArea", "FunctionFlag",
        "AccessFlag", "Password", "Longitude_WGS84", "Latitude_WGS84", "geodata_center", "geoarea" };
    
    public static readonly List<string> valuesChoice = new() { "ID_choice", "global_id_choice", "Name_choice",
        "AdmArea_choice", "District_choice", "ParkName_choice", "WifiName_choice", "CoverageArea_choice", 
        "FunctionFlag_choice", "AccessFlag_choice", "Password_choice", "Longitude_WGS84_choice",
        "Latitude_WGS84_choice", "geodata_center_choice", "geoarea_choice" };
};