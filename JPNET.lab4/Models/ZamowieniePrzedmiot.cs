using System;
using System.Collections.Generic;
using System.Text;

namespace JPNET.lab4.Models
{
    public class ZamowieniePrzedmiot
    {
        public int Id { get; set; }
        public Przedmiot Przedmiot { get; set; }
        public Zamowienie Zamowienie { get; set; }
        public int Liczba { get; set; }
    }
}
