using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JPNET.lab4.Config;
using JPNET.lab4.DAL;
using JPNET.lab4.Models;
using Microsoft.EntityFrameworkCore;
using ApplicationContext = JPNET.lab4.DAL.ApplicationContext;

namespace JPNET.lab4.Services
{
    public class ZamowienieService
    {
        public void ShowOperationScreen()
        {
            ConsoleKeyInfo input;
            ShowGeneralMenu();
            do
            {
                input = Console.ReadKey();
                HandleKey(input);
                ShowGeneralMenu();

            } while (input.Key != ConsoleKey.D4);
        }

        private void ShowGeneralMenu()
        {
            Console.Clear();
            Console.WriteLine("1.Dodaj zamowienie");
            Console.WriteLine("2.Dodaj zamowienie internetowe");
            Console.WriteLine("3.Przegladaj zamowienia");
            Console.WriteLine("4.Wroc do menu");

            Console.WriteLine($"\n ID aktywnego klienta: {Context.CurrentClientId}");
        }

        private void HandleKey(ConsoleKeyInfo cki)
        {
            switch (cki.Key)
            {
                case ConsoleKey.D1:
                    AddOrder();
                    break;
                case ConsoleKey.D2:
                    AddInternetOrder();
                    break;
                case ConsoleKey.D3:
                    ShowOrders();
                    break;
                default:
                    break;
            }
        }

        private void AddOrder()
        {
            Console.Clear();
            using ApplicationContext context = new ApplicationContext();
            List<ZamowieniePrzedmiot> zamowieniePrzedmiot = new List<ZamowieniePrzedmiot>();
            Console.WriteLine("Wpisz cokolwiek innego poza liczbą, aby przestać dodawac przedmioty");

            while (true)
            {
                var przedmiot = new ZamowieniePrzedmiot();
                Console.WriteLine($"Wpisz numer id przedmiotu");
                string input = Console.ReadLine();
                if (int.TryParse(input, out int i))
                {
                    przedmiot.Przedmiot = context.Przedmioty.Find(i);
                }
                else
                {
                    break;
                }
                Console.WriteLine($"Wpisz liczbę przedmiotów");

                input = Console.ReadLine();
                if (int.TryParse(input, out int j))
                {
                    przedmiot.Liczba = j;
                }
                else
                {
                    break;
                }

                zamowieniePrzedmiot.Add(przedmiot);
            }
            if (!zamowieniePrzedmiot.Any())
            {
                Console.WriteLine("Nie mozna dodac pustego zamowienia!");
                Console.WriteLine("Wcisnij dowolny przycisk aby kontynuowac.");
                Console.ReadKey();
                return;
            }
            var klient = context.Klienci.Find(Context.CurrentClientId);

            Zamowienie zamowienie = new Zamowienie()
            {
                Klient = klient,
                Zrealizowane = false
            };
            context.Zamowienia.Add(zamowienie);
            //context.SaveChanges();
            foreach (var zp in zamowieniePrzedmiot)
            {

                zp.Zamowienie = zamowienie;
                context.ZamowieniaPrzedmiotow.Add(zp);
            }

            context.SaveChanges();
            Console.WriteLine("Dodano zamowienie!");
            Console.WriteLine("Wcisnij dowolny przycisk aby kontynuowac.");
            Console.ReadKey();
        }

        private void AddInternetOrder()
        {
            Console.Clear();
            using ApplicationContext context = new ApplicationContext();
            List<ZamowieniePrzedmiot> zamowieniePrzedmiot = new List<ZamowieniePrzedmiot>();
            Console.WriteLine("Wpisz adres ip");
            string ip = Console.ReadLine();

            Console.WriteLine("Wpisz cokolwiek innego poza liczbą, aby przestać dodawac przedmioty");
            while (true)
            {
                var przedmiot = new ZamowieniePrzedmiot();
                Console.WriteLine($"Wpisz numer id przedmiotu");
                string input = Console.ReadLine();
                if (int.TryParse(input, out int i))
                {
                    przedmiot.Przedmiot = context.Przedmioty.Find(i);
                }
                else
                {
                    break;
                }
                Console.WriteLine($"Wpisz liczbę przedmiotów");

                input = Console.ReadLine();
                if (int.TryParse(input, out int j))
                {
                    przedmiot.Liczba = j;
                }
                else
                {
                    break;
                }

                zamowieniePrzedmiot.Add(przedmiot);
            }
            if(!zamowieniePrzedmiot.Any())
            {
                Console.WriteLine("Nie mozna dodac pustego zamowienia!");
                Console.WriteLine("Wcisnij dowolny przycisk aby kontynuowac.");
                Console.ReadKey();
                return;
            }

            var klient = context.Klienci.Find(Context.CurrentClientId);

            ZamowienieInternetowe zamowienie = new ZamowienieInternetowe()
            {
                Klient = klient,
                Zrealizowane = false,
                Ip = ip
            };
            context.ZamowieniaInternetowe.Add(zamowienie);
            context.SaveChanges();
            foreach (var zp in zamowieniePrzedmiot)
            {

                zp.Zamowienie = zamowienie;
                context.ZamowieniaPrzedmiotow.Add(zp);
            }

            context.SaveChanges();
            Console.WriteLine("Dodano zamowienie!");
            Console.WriteLine("Wcisnij dowolny przycisk aby kontynuowac.");
            Console.ReadKey();
        }

        private void RealizeOrder(Zamowienie zamowienie)
        {
            if(zamowienie.Zrealizowane)
            {
                Console.WriteLine("Zamowienie jest juz zrealizowane!");
                Console.WriteLine("Wcisnij dowolny przycisk aby kontynuowac.");
                Console.ReadKey();
                return;
            }
            using ApplicationContext context = new ApplicationContext();
            var zamowieniePrzedmiot = context.ZamowieniaPrzedmiotow.Include(x => x.Przedmiot).Where(x => x.Zamowienie.Id == zamowienie.Id).ToList();
            using var transaction = context.Database.BeginTransaction();
            bool success = true;
            transaction.CreateSavepoint("realizeorder");
            foreach (var zp in zamowieniePrzedmiot)
            {
                if (zp.Liczba > zp.Przedmiot.Amount)
                {
                    Console.WriteLine("Nie udało się zrealizować zamówienia! Brak wystarczającej liczby przedmiotu w magazynie");
                    success = false;
                    break;
                }
                zp.Przedmiot.Amount -= zp.Liczba;
            }
            if(success)
            {
                zamowienie.Zrealizowane = true;
                context.SaveChanges();
                Console.WriteLine("Pomyslnie zraelizowano zamowienie!");
            }
            else
            {
                transaction.RollbackToSavepoint("realizeorder");
            }

            transaction.Commit();
            Console.WriteLine("Wcisnij dowolny przycisk aby kontynuowac.");
            Console.ReadKey();
        }

        private void ShowOrders()
        {
            int currentPage = 1;
            Console.Clear();
            StringBuilder sb = new StringBuilder();
            ConsoleKeyInfo input;
            using ApplicationContext context = new ApplicationContext();
            List<Zamowienie> zamowienia = new List<Zamowienie>();
            do
            {
                Console.Clear();
                Console.WriteLine("P - poprzednia strona, N - nastepna strona, ESC - wyjscie, wcisnij numer wg kolejnosci na WIDOCZNEJ liscie, zeby zrealizowac zamowienie");
                Console.WriteLine($"Obecna strona : {currentPage}");
                zamowienia = context.Zamowienia.Include(x => x.Klient).Include(x => x.Przedmioty).ThenInclude(x => x.Przedmiot).Skip((currentPage - 1) * 5).Take(5).ToList();
                ShowZamowienia(zamowienia, currentPage);
                input = Console.ReadKey();
                if (input.Key == ConsoleKey.N)
                {
                    currentPage++;
                }
                else if (input.Key == ConsoleKey.P)
                {
                    if (currentPage > 1)
                    {
                        currentPage--;
                    }
                }
                if (int.TryParse(input.KeyChar.ToString(), out int i))
                {
                    try
                    {
                        var zamowienie = zamowienia[i - 1];
                        RealizeOrder(zamowienie);
                    }
                    catch(Exception)
                    {
                        Console.WriteLine("Problem przy realizacji zamowienia! Wcisnij dowolny przycisk aby kontynuowac.");
                        Console.ReadKey();
                    }
                }

            } while (input.Key != ConsoleKey.Escape);
            ShowOperationScreen();
        }

        private static void ShowZamowienia(List<Zamowienie> zamowienia, int currentPage)
        {
            int i = (currentPage - 1) * 5;
            zamowienia.ForEach(k => Console.WriteLine($"{++i}.  ID:{k.Id}   Klient: {k.Klient.Nazwa}    Cena calkowita:{k.CenaCalkowita}    Liczba przedmiotow: {k.LiczbaCalkowita} Internetowe:{k.GetType() == typeof(ZamowienieInternetowe)}  Zrealizowane: {k.Zrealizowane}\n"));
        }
    }
}
