namespace GreenZoneWifiBot.Utils;

public static class PathGetter
{
    public static string Get(string dirName)
    {
        var curenntDir = new DirectoryInfo(Directory.GetCurrentDirectory());
        var varParrentPath = curenntDir.Parent ?? curenntDir;
        var path = Path.Combine(varParrentPath.FullName, dirName);

        return path;
    }
}