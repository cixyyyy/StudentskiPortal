using Microsoft.AspNetCore.Mvc;
using PrijavaIspita.Models;
using PrijavaIspita.Models.EFRepository;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System;
using Microsoft.AspNetCore.Http.HttpResults;

namespace PrijavaIspita.Controllers
{
    public class PrijavaIspitaController : Controller
    {
        private readonly PrijavaIspitaRepository _prijavaIspitaRepository;

        public PrijavaIspitaController()
        {
            _prijavaIspitaRepository = new PrijavaIspitaRepository();
        }

        // GET - PrijavaIspita/Index
        public IActionResult Index()
        {
            int? korisnikId = HttpContext.Session.GetInt32("KorisnikID");
            if (korisnikId == null)
                return RedirectToAction("Login", "Account", new { ReturnUrl = "/PrijavaIspita/Index" });

            // Dohvati korisnika iz baze
            var korisnik = _prijavaIspitaRepository.GetAllKorisnici()
                .FirstOrDefault(s => s.Id == korisnikId.Value);
            if (korisnik == null)
                return RedirectToAction("Login", "Account");
            
                // Student vidi samo predmete svog smera
                var predmeti = _prijavaIspitaRepository.GetAllPredmeti()
                            .Where(p => p.SmerId == korisnik.Smer.Id)
                            .ToList();
            

            ViewBag.Predmeti = predmeti;
            ViewBag.Rokovi = _prijavaIspitaRepository.GetRokovi();
            ViewBag.StanjeBudzet = _prijavaIspitaRepository.GetBudzet(korisnik.Id);
            // Dohvati sve prijave ovog korisnika
            var prijave = _prijavaIspitaRepository.GetByKorisnikId(korisnik.Id);

            return View(prijave);
        }

        // Filtriranje ispita po predmetu
        public IActionResult GetIspiteByPredmet(int id)
        {
            int? korisnikId = HttpContext.Session.GetInt32("KorisnikID");
            if (korisnikId == null)
                return RedirectToAction("Login", "Account");

            var prijave = _prijavaIspitaRepository.GetByKorisnikId(korisnikId.Value);

            if (id != 0)
                prijave = prijave.Where(p => p.Predmet.Id == id).ToList();

            return PartialView("ListaIspita", prijave);
        }

        // POST - prijava predmeta
        [HttpPost]
        public IActionResult PrijaviPredmet(int PredmetId, string Rok)
        {
            int? korisnikId = HttpContext.Session.GetInt32("KorisnikID");
            if (korisnikId == null)
                return RedirectToAction("Login", "Account");

            // Provera da li je rok selektovan
            if (string.IsNullOrEmpty(Rok))
            {
                TempData["Greska"] = "Morate odabrati rok!";
                return RedirectToAction("Index");
            }

            var korisnik = _prijavaIspitaRepository.GetAllKorisnici()
                                .FirstOrDefault(s => s.Id == korisnikId.Value);
            var predmet = _prijavaIspitaRepository.GetAllPredmeti()
                                .FirstOrDefault(p => p.Id == PredmetId);

            if (korisnik == null || predmet == null)
            {
                TempData["Greska"] = "Korisnik ili predmet nisu pronađeni.";
                return RedirectToAction("Index");
            }

            bool postojiPrijava = _prijavaIspitaRepository.GetByKorisnikId(korisnik.Id)
                                    .Any(p => p.Predmet.Id == PredmetId);
            if (postojiPrijava)
            {
                TempData["Greska"] = "Već ste prijavili ovaj predmet.";
                return RedirectToAction("Index");
            }

            double budzet = _prijavaIspitaRepository.GetBudzet(korisnik.Id);
            if (budzet < 500)
            {
                TempData["Greska"] = "Nemate dovoljno sredstava za prijavu ispita!";
                return RedirectToAction("Index");
            }

            // Dodaj prijavu ispita
            var prijava = new PrijavaIspitaBO
            {
                Korisnik = korisnik,
                Predmet = predmet,
                Rok = Rok,
                DatumPrijave = DateTime.Now
            };
            _prijavaIspitaRepository.Add(prijava);

            // Dodaj obavestenja u bazu
            _prijavaIspitaRepository.DodajObavestenje(korisnik.Id, $"Uspešno ste prijavili ispit: {predmet.Naziv}");

            TempData["Uspesno"] = $"Uspešno ste prijavili ispit: {predmet.Naziv}";

            return RedirectToAction("Index");
        }

        // Slanje zahteva za budzet
        [HttpPost]
        public IActionResult PosaljiZahtevBudzet(decimal kolicina)
        {
            int? korisnikId = HttpContext.Session.GetInt32("KorisnikID");
            if (korisnikId == null)
                return RedirectToAction("Login", "Account");

            _prijavaIspitaRepository.DodajZahtevBudzet(korisnikId.Value, kolicina);

            // Dodajemo obavestenje za poslat zahtev
            _prijavaIspitaRepository.DodajObavestenje(korisnikId.Value, $"Uspesno ste poslali zahtev za budzet u iznosu od: {kolicina} RSD");

            TempData["Uspesno"] = "Uspešno ste poslali zahtev za budžet!";
            return RedirectToAction("Index");
        }


    }
}
