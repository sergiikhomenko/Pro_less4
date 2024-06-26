using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        var items = new List<(string Name, decimal Price)>
        {
            ("Товар 1", 10.50m),
            ("Товар 2", 5.75m),
            ("Товар 3", 20.00m),
            ("Товар 4", 8.30m)
        };
        DateTime purchaseDate = DateTime.Now;
        
        string filePath = "receipt.txt";
        
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("Дата покупки: " + purchaseDate.ToString("G"));
            writer.WriteLine("Чек:");

            foreach (var item in items)
            {
                writer.WriteLine($"{item.Name} – {item.Price:0.00} грн.");
            }
        }
        
        string receiptContent = File.ReadAllText(filePath);
        
        Console.WriteLine("Формат поточної локалі користувача:");
        Console.WriteLine(receiptContent);
        
        CultureInfo enUS = new CultureInfo("en-US");
        string enUSReceiptContent = ConvertReceiptToCulture(receiptContent, enUS);

        Console.WriteLine("\nФормат локалі en-US:");
        Console.WriteLine(enUSReceiptContent);
    }

    static string ConvertReceiptToCulture(string receiptContent, CultureInfo culture)
    {
        string[] lines = receiptContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

        DateTime purchaseDate = DateTime.Parse(lines[0].Replace("Дата покупки: ", ""));
        string formattedDate = "Дата покупки: " + purchaseDate.ToString("G", culture);

        var formattedLines = new List<string> { formattedDate, lines[1] };

        for (int i = 2; i < lines.Length; i++)
        {
            string line = lines[i];
            if (line.Contains("грн."))
            {
                int priceIndex = line.LastIndexOf(" – ") + 3;
                string pricePart = line.Substring(priceIndex).Replace(" грн.", "");
                if (decimal.TryParse(pricePart, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal price))
                {
                    string formattedLine = line.Substring(0, priceIndex) + price.ToString("0.00", culture) + " грн.";
                    formattedLines.Add(formattedLine);
                }
                else
                {
                    formattedLines.Add(line); 
                }
            }
            else
            {
                formattedLines.Add(line); 
            }
        }

        return string.Join(Environment.NewLine, formattedLines);
    }
}