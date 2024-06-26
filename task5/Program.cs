using System.Text.RegularExpressions;

namespace task5;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Введіть логін");
            string login = Console.ReadLine();

            Console.WriteLine("Введіть пароль");
            string password = Console.ReadLine();

            if (CheckLogin(login)&& CheckPassword(password))
            {
                Console.WriteLine("Ура");
            }
            else
            {
                Console.WriteLine("Спробуй ще");
            }
        }
       
    }

    static bool CheckLogin(string login)
    {
        return Regex.IsMatch(login, "^[a-zA-Z]+$");
    }

    static bool CheckPassword(string password)
    {
        return Regex.IsMatch(password, "^[0-9!@#$%^&*()_+=-]+$");
    }
}