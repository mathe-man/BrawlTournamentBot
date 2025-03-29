using System;
using System.IO;
using Discord;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

using ClosedXML.Excel;

namespace BrawlTournamentBot;

public class Excel
{
    public static void RunDB()
    {
        string filePath = "DB/fiche de calcul.xlsx";

        using (var workbook = new XLWorkbook())  // Crée un nouveau fichier Excel
        {
            var sheet = workbook.Worksheets.Add("Feuille1"); // Ajoute une feuille

            // Ajouter des données dans les cellules
            sheet.Cell("A1").Value = "Nom";
            sheet.Cell("B1").Value = "Âge";
            sheet.Cell("A2").Value = "Alice";
            sheet.Cell("B2").Value = 25;
            sheet.Cell("A3").Value = "Bob";
            sheet.Cell("B3").Value = 30;

            // Sauvegarde du fichier Excel
            workbook.SaveAs(filePath);
        }

        Console.WriteLine($"Fichier Excel créé : {filePath}");
    }
}

public class DataBase
{
    public readonly string DataBaseFolder;
    public List<Player> Players = new ();

    public DataBase(string dbFolderPath = "DB")
    {
        DataBaseFolder = dbFolderPath.EndsWith('/') ? dbFolderPath : dbFolderPath + "/";
        
        if (Directory.Exists(DataBaseFolder))   Console.WriteLine($"Directory exist:{new DirectoryInfo(DataBaseFolder).FullName}");
        else Console.WriteLine($"Directory created:{Directory.CreateDirectory(DataBaseFolder).FullName}");

        
        if (File.Exists("players.json")) Players = LoadDbFile<Player>("players.json");
        else Players = SaveDbFile<Player>("players.json", new ());
    }


    /// <summary>
    /// Get a json formated text file and return a Dictionary of values
    /// </summary>
    /// <param name="file">The json file, it needs to be in the DataBaseFolder</param>
    /// <returns>A Dictionary(string, string) representing the file content</returns>
    /// <exception cref="FileNotFoundException">If the given file path is not found in the DataBaseFolder</exception>
    public List<T> LoadDbFile<T>(string file)
    {
        if (!File.Exists(file))
            throw new FileNotFoundException($"The file is not in the DB folder({DataBaseFolder}{file})", file);
        
        string content = File.ReadAllText(file);
        List<T>? result = JsonConvert.DeserializeObject<List<T>>(DataBaseFolder + file);
        
        result ??= new();
        return result;
    }

    
    /// <summary>
    /// Save a Dictionary(string, string) to a file with a json formating indented formatting
    /// </summary>
    /// <param name="file">The where the data will be saved, it's created if not found</param>
    /// <param name="data">A Dictionary of string to save</param>
    /// <returns>The same Dictionary given in parameters</returns>
    public List<T> SaveDbFile<T>(string file, List<T> data)
    {
        if (!File.Exists(file))
        {
            FileStream fs = File.Create(DataBaseFolder + file); fs.Close();
        }
        
        string jsonString = JsonConvert.SerializeObject(data, Formatting.Indented);
        
        File.WriteAllText(DataBaseFolder + file, jsonString);

        return data;
    }
}



public class Player
{
    [JsonPropertyName("Tag")]
    public string Tag { get; set; }
    
    [JsonPropertyName("SupercellID")]
    public string SupercellID { get; set; }
    
    [JsonPropertyName("BrawlName")]
    public string BrawlName { get; set; }
    
    [JsonPropertyName("DiscordName")]
    public string DiscordUserName { get; set; }
    
    [JsonPropertyName("DiscordId")]
    public string DiscordId { get; set; }
    
    [JsonPropertyName("Team")]
    public string? TeamName { get; set; }

    public Player(string brawlName, string tag, string supercellId, string discordName, string discordId)
    {
        BrawlName = brawlName;
        SupercellID = supercellId;
        Tag = tag;
        DiscordId = discordId;
        DiscordUserName = discordName;
    }

}
