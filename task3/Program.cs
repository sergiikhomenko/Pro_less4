using System.Net.Mime;

namespace task3;

class Program
{
    static void Main(string[] args)
    {
        var prepositions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "без", "в", "від", "до", "за", "з", "і", "й", "на", "по", "під", "про", "у", "через"
        };
        string path = "text.txt";
        string newText;
        if (File.Exists(path))
        {
            string text = File.ReadAllText(path);

            string[] words = text.Split(new char[] { ' ', '\r', '\n', '\t' });

            for (int  i = 0;  i < words.Length;  i++)
            {
                string clearWords = words[i].Trim('.', ',', ';', ':', '!', '?' );

                if (prepositions.Contains(clearWords))
                {
                    words[i] = "ГАВ";
                }
            }

            newText = string.Join(" ", words);
            string newFail = "New_Text.txt";
            File.WriteAllText(newFail,newText);
        }
        else
        {
            Console.WriteLine("Невдалось прочитати  файл");
        }
     
    }
}