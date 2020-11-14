using JPNET.lab4.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace JPNET.lab4.DAL
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Przedmiot> Przedmioty { get; set; }
        public DbSet<Klient> Klienci { get; set; }
        public DbSet<KlientInternetowy> KlienciInternetowi { get; set; }
        public DbSet<Zamowienie> Zamowienia { get; set; }
        public DbSet<ZamowienieInternetowe> ZamowieniaInternetowe { get; set; }
        public DbSet<ZamowieniePrzedmiot> ZamowieniaPrzedmiotow { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=JPNET_lab4;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Klient>()
                .HasDiscriminator<string>("klient_type")
                .HasValue<Klient>("klient")
                .HasValue<KlientInternetowy>("klient_internetowy");

            modelBuilder.Entity<Zamowienie>().ToTable("zamowienia");

            modelBuilder.Entity<ZamowienieInternetowe>().ToTable("zamowienia_internetowe");

            //List<Klient> klienci = new List<Klient>();
            //List<KlientInternetowy> klienciInternetowi = new List<KlientInternetowy>();
            //for (int i = 1; i <= 10; i++)
            //{
            //    klienci.Add(new Klient() {Id= i, Adres = Faker.Address.City(), Nazwa = Faker.Company.Name() });
            //    klienciInternetowi.Add(new KlientInternetowy() { Id = i * 2, Adres = Faker.Address.City(), Nazwa = Faker.Company.Name(), Ip = Faker.Internet.DomainName() }) ;
            //} 

            //modelBuilder.Entity<Klient>()
            //    .HasData(klienci.ToArray());

            //modelBuilder.Entity<KlientInternetowy>()
            //    .HasData(klienciInternetowi.ToArray());
        }


    }
}
