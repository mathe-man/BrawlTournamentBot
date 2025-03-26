using System;
using System.IO;

using ClosedXML.Excel;

namespace BrawlTournamentBot;

public class DataBase
{
    public static void RunDB()
    {
        string filePath = "mon_fichier.xlsx";

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