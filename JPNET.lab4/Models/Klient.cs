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
            return context.Zamowienia.Where(p => p.Zrealizowane && p.Klient.Id == Id).Select(p => p.CenaCalkowita).Sum();
        }

    }
}
