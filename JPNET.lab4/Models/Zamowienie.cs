using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JPNET.lab4.Models
{
    public class Zamowienie
    {
        public int Id { get; set; }
        public Klient Klient { get; set; }
        public bool Zrealizowane { get; set; }
        public List<ZamowieniePrzedmiot> Przedmioty { get; set; } = new List<ZamowieniePrzedmiot>();
        public int LiczbaCalkowita => Przedmioty.Select(x => x.Liczba).Sum();
        public decimal CenaCalkowita => Przedmioty.Select(x => x.Przedmiot.Price * x.Liczba).Sum();
    }
}
