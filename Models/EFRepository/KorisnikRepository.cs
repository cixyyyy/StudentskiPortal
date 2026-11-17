using System.Collections.Generic;
using System.Linq;

namespace PrijavaIspita.Models.EFRepository
{
    public class KorisnikRepository : IKorisnikRepository
    {
        private PrijavaIspitaContext prijavaIspitaEntities = new PrijavaIspitaContext();

        // Dohvatanje svih studenata
        public IEnumerable<KorisnikBO> GetAll()
        {
            List<KorisnikBO> studenti = new List<KorisnikBO>();

            foreach (var s in prijavaIspitaEntities.Korisnici.ToList())
            {
                studenti.Add(new KorisnikBO
                {
                    Id = s.KorisnikID,
                    Ime = s.Ime,
                    Prezime = s.Prezime,
                    Smer = new SmerBO
                    {
                        Id = s.Smer.SmerId,
                        Naziv = s.Smer.Naziv
                    }
                });
            }

            return studenti;
        }

        // Dohvatanje studenta po ID
        public KorisnikBO GetById(int id)
        {
            var s = prijavaIspitaEntities.Korisnici.FirstOrDefault(st => st.KorisnikID == id);
            if (s == null) return null!;

            return new KorisnikBO
            {
                Id = s.KorisnikID,
                Ime = s.Ime,
                Prezime = s.Prezime,
                Smer = new SmerBO
                {
                    Id = s.Smer.SmerId,
                    Naziv = s.Smer.Naziv
                }
            };
        }

        // Dodavanje novog studenta
        public void Add(KorisnikBO student)
        {
            var s = new Korisnik
            {
                Ime = student.Ime,
                Prezime = student.Prezime,
                SmerId = student.Smer.Id
            };

            prijavaIspitaEntities.Korisnici.Add(s);
            prijavaIspitaEntities.SaveChanges();
        }

        // Azuriranje postojeceg studenta
        public void Update(KorisnikBO student)
        {
            var s = prijavaIspitaEntities.Korisnici.FirstOrDefault(st => st.KorisnikID == student.Id);
            if (s != null)
            {
                s.Ime = student.Ime;
                s.Prezime = student.Prezime;
                s.SmerId = student.Smer.Id;

                prijavaIspitaEntities.SaveChanges();
            }
        }

        // Brisanje studenta
        public void Delete(int id)
        {
            var s = prijavaIspitaEntities.Korisnici.FirstOrDefault(st => st.KorisnikID == id);
            if (s != null)
            {
                prijavaIspitaEntities.Korisnici.Remove(s);
                prijavaIspitaEntities.SaveChanges();
            }
        }
    }
}
