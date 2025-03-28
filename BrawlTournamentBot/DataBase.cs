using System;
using System.IO;
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
    public Dictionary<string, string> Players = new ();

    public DataBase(string dbFolderPath = "DB")
    {
        DataBaseFolder = dbFolderPath.EndsWith('/') ? dbFolderPath : dbFolderPath + "/";
        
        if (Directory.Exists(DataBaseFolder))   Console.WriteLine($"Directory exist:{new DirectoryInfo(DataBaseFolder).FullName}");
        else Console.WriteLine($"Directory created:{Directory.CreateDirectory(DataBaseFolder).FullName}");

        
        if (File.Exists("players.json")) Players = LoadDbFile("players.json");
        else Players = SaveDbFile("players.json", new ());
    }


    /// <summary>
    /// Get a json formated text file and return a Dictionary of values
    /// </summary>
    /// <param name="file">The json file, it needs to be in the DataBaseFolder</param>
    /// <returns>A Dictionary(string, string) representing the file content</returns>
    /// <exception cref="FileNotFoundException">If the given file path is not found in the DataBaseFolder</exception>
    private Dictionary<string, string> LoadDbFile(string file)
    {
        if (!File.Exists(file))
            throw new FileNotFoundException($"The file is not in the DB folder({DataBaseFolder}{file})", file);
        
        string content = File.ReadAllText(file);
        Dictionary<string, string>? result = JsonConvert.DeserializeObject<Dictionary<string, string>>(DataBaseFolder + file);
        
        result ??= new();
        return result;
    }

    private Dictionary<string, string> SaveDbFile(string file, Dictionary<string, string> data)
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


