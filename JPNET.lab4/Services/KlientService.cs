using AutoFixture;
using JPNET.lab4.Config;
using JPNET.lab4.DAL;
using JPNET.lab4.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApplicationContext = JPNET.lab4.DAL.ApplicationContext;

namespace JPNET.lab4.Services
{
    public class KlientService
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

            } while (input.Key != ConsoleKey.D2);
        }

        private void ShowGeneralMenu()
        {
            Console.Clear();
            Console.WriteLine("1.Ustaw aktywnego klienta");
            Console.WriteLine("2.Wroc do menu");

            Console.WriteLine($"\n ID aktywnego klienta: {Context.CurrentClientId}");
        }

        private void HandleKey(ConsoleKeyInfo cki)
        {
            switch (cki.Key)
            {
                case ConsoleKey.D1:
                    SearchForCustomer();
                    break;
                default:
                    break;
            }
        }

        private void SearchForCustomer()
        {
            Console.Clear();
            StringBuilder sb = new StringBuilder();
            ConsoleKeyInfo input;
            using ApplicationContext context = new ApplicationContext();
            List<Klient> klienci = new List<Klient>();
            Console.WriteLine($"Wpisz nazwę klienta: {sb}");
            do
            {
                input = Console.ReadKey();
                if (int.TryParse(input.KeyChar.ToString(), out int i))
                {
                    Context.CurrentClientId = klienci[i].Id;
                    break;
                }
                else
                {
                    sb.Append(input.KeyChar);
                    Console.Clear();
                    Console.WriteLine($"Wpisz nazwę klienta: {sb}");
                    klienci = context.Klienci.Where(k => EF.Functions.Like(k.Nazwa, $"%{sb}%")).OrderBy(k => k.Id).Take(10).ToList();
                    ShowKlienci(klienci);
                }
            } while (input.Key != ConsoleKey.Escape);
            ShowOperationScreen();
        }


        private static void ShowKlienci(List<Klient> klienci)
        {
            int i = 0;
            klienci.ForEach(k => Console.WriteLine($"{i++}. {k.Nazwa}"));
        }
    }
}