using Microsoft.EntityFrameworkCore;
using PrijavaIspita.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrijavaIspita.Models.EFRepository
{
    public class PrijavaIspitaRepository : IPrijavaIspitaRepository
    {
        private PrijavaIspitaContext prijavaIspitaEntities = new PrijavaIspitaContext();

        // Dodavanje prijave ispita
        public void Add(PrijavaIspitaBO prijavaIspita)
        {
            Korisnik korisnik = prijavaIspitaEntities.Korisnici
                .SingleOrDefault(s => s.KorisnikID == prijavaIspita.Korisnik.Id);

            Budzet budzet = prijavaIspitaEntities.Budzets
                .SingleOrDefault(b => b.KorisnikID == korisnik.KorisnikID);

            if (budzet == null || budzet.Iznos < 500)
                throw new Exception("Nemate dovoljno sredstava za prijavu ispita!");

            Prijava prijavaModel = new Prijava
            {
                KorisnikID = prijavaIspita.Korisnik.Id,
                PredmetId = prijavaIspita.Predmet.Id,
                Ocena = prijavaIspita.Ocena,
                Rok = prijavaIspita.Rok,
                DatumPrijave = DateTime.Now
            };
            prijavaIspitaEntities.Prijavas.Add(prijavaModel);

            budzet.Iznos -= 500;

            prijavaIspitaEntities.Transakcijas.Add(new Transakcija
            {
                KorisnikId = korisnik.KorisnikID,
                Iznos = 500,
                Tip = "Prijava",
                Datum = DateTime.Now
            });

            prijavaIspitaEntities.SaveChanges();
        }

        // Dohvatanje svih prijava
        public IEnumerable<PrijavaIspitaBO> GetAll()
        {
            return prijavaIspitaEntities.Prijavas
                .Include(p => p.Predmet)
                .ThenInclude(pred => pred.Profesor) // INCLUDE profesora
                .Include(p => p.Korisnik)
                .ThenInclude(s => s.Smer)
                .Select(p => new PrijavaIspitaBO
                {
                    Ocena = p.Ocena,
                    Rok = p.Rok,
                    Predmet = new PredmetBO
                    {
                        Id = p.Predmet.PredmetId,
                        Naziv = p.Predmet.Naziv,
                        ProfesorID = (int)p.Predmet.ProfesorID,
                        ProfesorIme = p.Predmet.Profesor != null ? p.Predmet.Profesor.Ime : "",
                        ProfesorPrezime = p.Predmet.Profesor != null ? p.Predmet.Profesor.Prezime : ""
                    },
                    Korisnik = new KorisnikBO
                    {
                        Id = p.Korisnik.KorisnikID,
                        Ime = p.Korisnik.Ime,
                        Prezime = p.Korisnik.Prezime,
                        GodinaUpisa = p.Korisnik.GodinaUpisa,
                        Smer = new SmerBO
                        {
                            Id = p.Korisnik.Smer.SmerId,
                            Naziv = p.Korisnik.Smer.Naziv
                        }
                    }
                }).ToList();
        }

        // Dohvatanje svih predmeta
        public IEnumerable<PredmetBO> GetAllPredmeti()
        {
            return prijavaIspitaEntities.Predmet
                .Include(p => p.Profesor)
                .Select(p => new PredmetBO
                {
                    Id = p.PredmetId,
                    Naziv = p.Naziv,
                    ProfesorID = (int)p.ProfesorID,
                    ProfesorIme = p.Profesor != null ? p.Profesor.Ime : "",
                    ProfesorPrezime = p.Profesor != null ? p.Profesor.Prezime : "",
                    SmerId = p.SmerId
                }).ToList();
        }

        // Dohvatanje predmeta po ID-u
        public IEnumerable<PrijavaIspitaBO> GetByPredmetId(int predmetId)
        {
            return GetAll().Where(p => p.Predmet.Id == predmetId);
        }

        // Dohvatanje korisnika po ID-u
        public IEnumerable<PrijavaIspitaBO> GetByKorisnikId(int korisnikId)
        {
            return GetAll().Where(p => p.Korisnik.Id == korisnikId);
        }

        // Dohvatanje svih korisnika
        public IEnumerable<KorisnikBO> GetAllKorisnici()
        {
            return prijavaIspitaEntities.Korisnici
                .Include(s => s.Smer)
                .Select(s => new KorisnikBO
                {
                    Id = s.KorisnikID,
                    Ime = s.Ime,
                    Prezime = s.Prezime,
                    GodinaUpisa = s.GodinaUpisa,
                    Smer = new SmerBO
                    {
                        Id = s.Smer.SmerId,
                        Naziv = s.Smer.Naziv
                    }
                }).ToList();
        }

        // Dohvatanje rokova
        public IEnumerable<string> GetRokovi()
        {
            return new List<string>
            {
                "Januarski",
                "Junski",
                "Julski",
                "Septembarski",
                "Oktobarski"
            };
        }

        // Dodavanje obavestenja
        public void DodajObavestenje(int korisnikId, string tekst)
        {
            if (!prijavaIspitaEntities.Korisnici.Any(s => s.KorisnikID == korisnikId))
                throw new Exception($"Korisnik sa ID {korisnikId} ne postoji u bazi!");

            prijavaIspitaEntities.Obavestenja.Add(new Obavestenje
            {
                KorisnikID = korisnikId,
                Tekst = tekst,
                Datum = DateTime.Now
            });
            prijavaIspitaEntities.SaveChanges();
        }

        //Dohvatanje obavestenja
        public IEnumerable<ObavestenjeBO> GetObavestenja(int korisnikId)
        {
            return prijavaIspitaEntities.Obavestenja
                .Where(o => o.KorisnikID == korisnikId)
                .OrderByDescending(o => o.Datum)
                .Select(o => new ObavestenjeBO
                {
                    Id = o.ObavestenjeId,
                    KorisnikID = o.KorisnikID,
                    Tekst = o.Tekst,
                    Datum = o.Datum
                }).ToList();
        }

        // Dohvatanje budzeta
        public double GetBudzet(int korisnikId)
        {
            var budzet = prijavaIspitaEntities.Budzets
                .FirstOrDefault(b => b.KorisnikID == korisnikId);
            return budzet?.Iznos ?? 0;
        }

        // Dodavanje zahteva za budzet
        public void DodajZahtevBudzet(int korisnikId, decimal kolicina)
        {
            var korisnik = prijavaIspitaEntities.Korisnici
                          .FirstOrDefault(s => s.KorisnikID == korisnikId);

            if (korisnik == null)
                throw new Exception($"Korisnik sa ID {korisnikId} ne postoji u bazi!");

            prijavaIspitaEntities.ZahtevBudzet.Add(new ZahtevBudzet
            {
                KorisnikId = korisnikId,
                Kolicina = kolicina,
                Datum = DateTime.Now,
                Status = "Na čekanju"
            });
            prijavaIspitaEntities.SaveChanges();
        }
        //Dohvatanje zahteva za budzet
        public IEnumerable<ZahtevBudzet> GetZahteviBudzet(int korisnikId)
        {
            return prijavaIspitaEntities.ZahtevBudzet
                .Where(z => z.KorisnikId == korisnikId)
                .OrderByDescending(z => z.Datum)
                .ToList();
        }

        // potrebno za uzimanje id profesora, jer svaki predmet ima vezanog profesora preko ProfesorID
        public KorisnikBO GetById(int id)
        {
            var p = prijavaIspitaEntities.Korisnici.FirstOrDefault(k => k.KorisnikID == id);
            if (p == null)
                throw new Exception("Profesor nije pronađen");

            return new KorisnikBO
            {
                Id = p.KorisnikID,
                Ime = p.Ime,
                Prezime = p.Prezime
            };
        }


    }
}
