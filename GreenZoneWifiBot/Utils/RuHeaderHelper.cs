namespace GreenZoneWifiBot.Utils;

public static class RuHeaderHelper
{
    private const string RuHeader = "Код;global_id;Наименование;Административный округ по адресу;Район;Наименование парка;Имя Wi-Fi сети;Зона покрытия (метры);Признак функционирования;Условия доступа;Пароль;Долгота в WGS-84;Широта в WGS-84;geodata_center;geoarea";
    
    public static async Task AddRuHeader(string path)
    {
        var initLines = File.ReadLines(path).ToList();

        await using var fileStream = File.OpenWrite(path);
        await using var writer = new StreamWriter(fileStream);

        for (int i = 0; i < initLines.Count; i++)
        {
            if (i == 1) { await writer.WriteLineAsync(RuHeader); }
            await writer.WriteLineAsync(initLines[i]);
        }
    }
}