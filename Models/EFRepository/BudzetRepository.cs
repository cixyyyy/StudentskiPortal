using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PrijavaIspita.Models.EFRepository
{
    public class BudzetRepository
    {
        private PrijavaIspitaContext prijavaIspitaEntities = new PrijavaIspitaContext();

        // Vrati sve zahteve sa korisnikom
        public IEnumerable<ZahtevBudzet> GetZahtevBudzetAll()
        {
            return prijavaIspitaEntities.ZahtevBudzet
                .Include(z => z.Korisnik)
                .ToList();
        }

        // Vrati zahtev po ID-u
        public ZahtevBudzet? GetZahtevBudzetById(int id)
        {
            return prijavaIspitaEntities.ZahtevBudzet
                .Include(z => z.Korisnik)
                .FirstOrDefault(z => z.ZahtevBudzetId == id);
        }

        // Promeni status zahteva - Odobri/Odbij zahtev
        public void UpdateZahtevBudzetStatus(int id, string status)
        {
            var z = prijavaIspitaEntities.ZahtevBudzet.FirstOrDefault(x => x.ZahtevBudzetId == id);
            if (z != null)
            {
                z.Status = status;
                prijavaIspitaEntities.SaveChanges();
            }
        }

        // Dodaje iznos iz zahteva korisnikovom budzetu
        public void AddToKorisnikBudzet(int korisnikId, decimal iznos)
        {
            var budzet = prijavaIspitaEntities.Budzets.FirstOrDefault(b => b.KorisnikID == korisnikId);
            var korisnik = prijavaIspitaEntities.Korisnici.FirstOrDefault(k => k.KorisnikID == korisnikId);

            if (budzet != null)
            {
                budzet.Iznos += (double)iznos;
            }
            else
            {
                prijavaIspitaEntities.Budzets.Add(new Budzet
                {
                    KorisnikID = korisnikId,
                    Iznos = (double)iznos,
                    Status = "Aktivan"
                });
            }

            if (korisnik != null)
            {
                korisnik.Budzet += (double)iznos; // update i u drugoj tabeli zbog greskje u postavci baze (imamo Budzet u 2 tabele)
            }

            prijavaIspitaEntities.SaveChanges();
        }

        //Dodaj obavestenje
        public void AddObavestenje(int korisnikId, string tekst)
        {
            prijavaIspitaEntities.Obavestenja.Add(new Obavestenje
            {
                KorisnikID = korisnikId,
                Tekst = tekst,
                Datum = DateTime.Now
            });
            prijavaIspitaEntities.SaveChanges();
        }


    }
}
