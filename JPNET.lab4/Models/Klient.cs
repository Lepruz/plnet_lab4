using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ApplicationContext = JPNET.lab4.DAL.ApplicationContext;

namespace JPNET.lab4.Models
{
    public class Klient
    {
        public int Id { get; set; }
        public string Nazwa { get; set; }
        public string Adres { get; set; }

        public decimal IleZamowil()
        {
            using ApplicationContext context = new ApplicationContext();
            var con = context.Zamowienia.Include(x => x.Klient).Include(x => x.Przedmioty).ThenInclude(x => x.Przedmiot).Where(p => p.Zrealizowane && p.Klient.Id == Id).ToList();
            return con.Select(p => p.CenaCalkowita).Sum();
        }

    }
}
