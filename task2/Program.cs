using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace task2;

class Program
{
    static async System.Threading.Tasks.Task Main(string[] args)
    {
        Console.WriteLine("Введіть адресу сайту");
        string url = Console.ReadLine();

        string htmlContent = await GetHtmlContent(url);
        if (!string.IsNullOrEmpty(htmlContent))
        {
            List<string> linksList = ExtractLinks(htmlContent);
            List<string> phoneList = ExtractPhone(htmlContent);
            List<string> emailList = ExtractEmail(htmlContent);
            
            SaveResultToFile(linksList,phoneList,emailList);
            Console.WriteLine("Результати було збережено у файл 'res.txt'.");
        }
        else
        {
            Console.WriteLine("Не вдалося отримати вміст веб-сторінки.");
        }

    }

    static async System.Threading.Tasks.Task<string> GetHtmlContent(string htmlContent)
    {
        using (HttpClient httpClient = new HttpClient())
        {
            try
            {
                HttpResponseMessage responseMessage = await httpClient.GetAsync(htmlContent);
                responseMessage.EnsureSuccessStatusCode();
                return await responseMessage.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Помилка при завантаженні веб-сторінки: {e.Message}");
                return string.Empty;
            }
            
        }
        
    }

    static List<string> ExtractLinks(string url)
    {
        List<string> links = new List<string>();
        var doc = new HtmlDocument();
        doc.LoadHtml(url);
        foreach (var link in doc.DocumentNode.SelectNodes("//a[@href]"))
        {
            var hrefValue = link.GetAttributeValue("href", String.Empty);
            if (!string.IsNullOrEmpty(hrefValue) && Uri.IsWellFormedUriString(hrefValue,UriKind.RelativeOrAbsolute))
            {
                links.Add(hrefValue);
            }
        }

        return links;
    }

    static List<string> ExtractPhone(string htmlContect)
    {
        List<string> listPhone = new List<string>();
        var phonePatern = @"[\+]\d{3}\s[\(]\d{2}[\)]\s\d{3}[\-]\d{2}[\-]\d{2}";

        var matches = Regex.Matches(htmlContect, phonePatern);
        
        foreach (Match match in matches)
        {
            listPhone.Add(match.Value);
        }
        
        return listPhone;
    }

    static List<string> ExtractEmail(string htmlContent)
    {
        List<string> listEmail = new List<string>();
        var patern = @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}";

        var matches = Regex.Matches(htmlContent, patern);

        foreach (Match mach in matches)
        {
            listEmail.Add(mach.Value);
        }
        return listEmail;
    }

    static void SaveResultToFile(List<string> links, List<string> phonesList, List<string> emailList)
    {
        using (StreamWriter streamWriter = new StreamWriter("res.txt"))
        {
            streamWriter.WriteLine("Links");
            foreach (var link in links)
            {
                streamWriter.WriteLine(link);
            }
            streamWriter.WriteLine("Phone");
            foreach (var phone in phonesList)
            {
                streamWriter.WriteLine(phone);
            }
            streamWriter.WriteLine("Email");
            foreach (var email in emailList)
            {
                streamWriter.WriteLine(email);
            }
            
            streamWriter.Close();
        }

        
    }
}