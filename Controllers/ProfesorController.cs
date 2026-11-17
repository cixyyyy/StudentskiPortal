using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrijavaIspita.Models;
using PrijavaIspita.Models.EFRepository;
using System.Text;
using System.Linq;

namespace PrijavaIspita.Controllers
{
    [Authorize(Roles = "Profesor")]
    public class ProfesorController : Controller
    {
        private PrijavaIspitaRepository prijavaRepository = new PrijavaIspitaRepository();

        // Pregled prijava po predmetu sa filterom po roku
        public IActionResult Index(string rok = "")
        {
            int? korisnikId = HttpContext.Session.GetInt32("KorisnikID");
            if (korisnikId == null)
                return RedirectToAction("Login", "Account");

            // Dohvati sve prijave za predmete koje profesor predaje
            var prijave = prijavaRepository.GetAll()
                .Where(p => p.Predmet.ProfesorID == korisnikId.Value);

            // Filtriranje po roku (ako je izabran)
            if (!string.IsNullOrEmpty(rok))
                prijave = prijave.Where(p => p.Rok == rok);

            // Grupisanje po predmetu i broj prijavljenih
            var model = prijave.GroupBy(p => p.Predmet.Naziv)
                .Select(g => new
                {
                    Predmet = g.Key,
                    BrojPrijavljenih = g.Count()
                })
                .ToList();

            ViewBag.ProfesorID = korisnikId.Value;
            ViewBag.Rokovi = prijavaRepository.GetRokovi();
            ViewBag.Rok = rok;

            return View(model);
        }

        // Pregled prijava
        public IActionResult PregledPrijava(string rok = "")
        {
            int? korisnikId = HttpContext.Session.GetInt32("KorisnikID");
            if (korisnikId == null)
                return RedirectToAction("Login", "Account");

            var prijave = prijavaRepository.GetAll()
                .Where(p => p.Predmet.ProfesorID == korisnikId.Value);

            if (!string.IsNullOrEmpty(rok))
                prijave = prijave.Where(p => p.Rok == rok);

            var model = prijave
                .GroupBy(p => p.Predmet.Naziv)
                .Select(g => new
                {
                    Predmet = g.Key,
                    BrojPrijavljenih = g.Count()
                })
                .ToList();

            ViewBag.ProfesorID = korisnikId.Value;
            ViewBag.Rokovi = prijavaRepository.GetRokovi();
            ViewBag.Rok = rok;

            return View(model);
        }

        // Export u CSV
        public IActionResult ExportCsv(string rok = "")
        {
            int? korisnikId = HttpContext.Session.GetInt32("KorisnikID");
            if (korisnikId == null)
                return RedirectToAction("Login", "Account");

            var prijave = prijavaRepository.GetAll()
                .Where(p => p.Predmet.ProfesorID == korisnikId.Value);

            if (!string.IsNullOrEmpty(rok))
                prijave = prijave.Where(p => p.Rok == rok);

            var csv = new StringBuilder();
            csv.AppendLine("Student,Predmet,Rok,Ocena");

            foreach (var p in prijave)
            {
                csv.AppendLine($"{p.Korisnik.Ime} {p.Korisnik.Prezime},{p.Predmet.Naziv},{p.Rok},{p.Ocena}");
            }

            return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", "Prijave.csv");
        }
    }
}
