using JPNET.lab4.DAL;
using JPNET.lab4.Models;
using JPNET.lab4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationContext = JPNET.lab4.DAL.ApplicationContext;

namespace JPNET.lab4
{
    class Program
    {
        private static readonly ItemService _itemService = new ItemService();
        private static readonly KlientService _klientService = new KlientService();
        private static readonly ZamowienieService _zamowienieService = new ZamowienieService();

        static void Main(string[] args)
        {
            SeedDb();
            ConsoleKeyInfo input;
            ShowMenu();
            do
            {
                input = Console.ReadKey();
                HandleKey(input);
                ShowMenu();
            } while (input.Key != ConsoleKey.Escape);
        }

        private static void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("1. Przedmioty");
            Console.WriteLine("2. Klienci");
            Console.WriteLine("3. Zamowienia");
        }

        private static void HandleKey(ConsoleKeyInfo cki)
        {
            switch (cki.Key)
            {
                case ConsoleKey.D1:
                    _itemService.ShowOperationScreen();
                    break;
                case ConsoleKey.D2:
                    _klientService.ShowOperationScreen();
                    break;
                case ConsoleKey.D3:
                    _zamowienieService.ShowOperationScreen();
                    break;
                default:
                    break;
            }
        }

        private static void SeedDb()
        {
            using ApplicationContext context = new ApplicationContext();
            if (context.Klienci.Any() || context.KlienciInternetowi.Any() || context.Zamowienia.Any())
                return;
            var random = new Random(); ;
            //List<Klient> klienci = new List<Klient>();
            //List<KlientInternetowy> klienciInternetowi = new List<KlientInternetowy>();
            for (int i = 1; i <= 10; i++)
            {
                context.Klienci.Add(new Klient() { Adres = Faker.Address.City(), Nazwa = Faker.Company.Name() });
                context.KlienciInternetowi.Add(new KlientInternetowy() { Adres = Faker.Address.City(), Nazwa = Faker.Company.Name(), Ip = Faker.Internet.DomainName() });
            }

            for (int i = 1; i <= 20; i++)
            {
                context.Przedmioty.Add(new Przedmiot() { Price = random.Next(100,10000)/100.0m, Amount = random.Next(0,5), Desc = Faker.Lorem.Sentence(3) });
            }

            //for (int i = 1; i <= 10; i++)
            //{
            //    context.Zamowienia.Add(new Zamowienie() { Klient = 1, = random.Next(100, 10000) / 100.0m, Amount = random.Next(0, 2), Desc = Faker.Lorem.Sentence(3) });
            //}

            context.SaveChanges();
        }
    }
}
