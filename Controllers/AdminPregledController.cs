using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrijavaIspita.Models;
using System.Linq;

namespace PrijavaIspita.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminPregledController : Controller
    {
        private readonly PrijavaIspitaContext _context;

        public AdminPregledController()
        {
            _context = new PrijavaIspitaContext();
        }

        public IActionResult Index()
        {
            return View();
        }
        //Ispis liste prijava za admina
        public IActionResult PrijaveAdmin()
        {
            var prijave = _context.Prijavas
                .Select(p => new PrijavaIspitaBO
                {
                    Id = p.PredmetId,
                    Ocena = p.Ocena,
                    Rok = p.Rok,
                    DatumPrijave = p.DatumPrijave,
                    Predmet = new PredmetBO
                    {
                        Id = p.Predmet.PredmetId,
                        Naziv = p.Predmet.Naziv,
                        ProfesorID = (int)p.Predmet.ProfesorID,
                        ProfesorIme = p.Predmet.Profesor.Ime,
                        ProfesorPrezime = p.Predmet.Profesor.Prezime
                    },
                    Korisnik = new KorisnikBO
                    {
                        Id = p.Korisnik.KorisnikID,
                        Ime = p.Korisnik.Ime,
                        Prezime = p.Korisnik.Prezime,
                        Email = p.Korisnik.Email
                    }
                }).ToList();

            return View(prijave);
        }

        //Ispis transakcija za admina
        public IActionResult TransakcijeAdmin()
        {
            var transakcije = _context.Transakcijas
                .Select(t => new TransakcijaBO
                {
                    Id = t.TransakcijaId,
                    Tip = t.Tip,
                    Iznos = (float)t.Iznos,
                    Datum = t.Datum,
                    Korisnik = new KorisnikBO
                    {
                        Id = t.Korisnik.KorisnikID,
                        Ime = t.Korisnik.Ime,
                        Prezime = t.Korisnik.Prezime,
                        Email = t.Korisnik.Email
                    }
                }).ToList();

            return View(transakcije);
        }

    }
}
