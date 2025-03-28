using System;
using System.IO;

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
    public readonly string FolderPath;
    public Dictionary<string, string> players = new ();

    public DataBase(string dbFolderPath = "DB")
    {
        FolderPath = dbFolderPath.EndsWith('/') ? dbFolderPath : dbFolderPath + "/";
        if (Directory.Exists(FolderPath))
        {
            Console.WriteLine($"Directory exist:{new DirectoryInfo(FolderPath).FullName}");
        }
        else
        {
            Console.WriteLine($"Directory created:{Directory.CreateDirectory(FolderPath).FullName}");
        }
    }
}


