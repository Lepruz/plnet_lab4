using System;
using AutoFixture;
using JPNET.lab4.DAL;
using JPNET.lab4.Models;
using ApplicationContext = JPNET.lab4.DAL.ApplicationContext;

namespace JPNET.lab4.Services
{
    public class ItemService
    {
        private int LastAddedId { get; set; }
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
            Console.WriteLine("1.Dodaj przedmiot");
            Console.WriteLine("2.Dodaj przedmiot (generuj)");
            Console.WriteLine("3.Zwieksz liczbe przedmiotow");
            Console.WriteLine("4.Wroc do menu");

            Console.WriteLine($"\n ID ostatnio dodanego przedmiotu: {LastAddedId}");
        }

        private void HandleKey(ConsoleKeyInfo cki)
        {
            switch (cki.Key)
            {
                case ConsoleKey.D1:
                    AddItem(false);
                    break;
                case ConsoleKey.D2:
                    AddItem(true);
                    break;
                case ConsoleKey.D3:
                    IncreaseAmount();
                    break;
                default:
                    break;
            }
        }

        private void AddItem(bool shouldGenerate)
        {
            Console.Clear();
            Przedmiot przedmiot;
            if (shouldGenerate)
            {
                var fixture = new Fixture();
                przedmiot = fixture.Create<Przedmiot>();
                przedmiot.Id = 0;
            }
            else
            {
                Console.WriteLine("Podaj cenę przedmiotu:");
                decimal price = Convert.ToDecimal(Console.ReadLine());
                Console.WriteLine("Podaj opis przedmiotu:");
                string desc = Console.ReadLine();
                Console.WriteLine("Podaj liczbę przedmiotów");
                int amount = Convert.ToInt32(Console.ReadLine());
                przedmiot = new Przedmiot()
                {
                    Desc = desc,
                    Price = price,
                    Amount = amount
                };
            }

            using ApplicationContext context = new ApplicationContext();
            context.Przedmioty.Add(przedmiot);
            context.SaveChanges();
            LastAddedId = przedmiot.Id;
        }

        private void IncreaseAmount()
        {
            Console.Clear();
            Console.WriteLine("Podaj id przedmiotu:");
            int id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Podaj liczbę przedmiotów");
            int amount = Convert.ToInt32(Console.ReadLine());

            using ApplicationContext context = new ApplicationContext();
            var przedmiot = context.Przedmioty.Find(id);

            przedmiot.Amount += amount;
            context.SaveChanges();
        }
    }
}

