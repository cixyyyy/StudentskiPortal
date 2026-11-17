using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrijavaIspita.Models;
using PrijavaIspita.Models.EFRepository;
using System.Linq;

namespace PrijavaIspita.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class BudzetController : Controller
    {
        private BudzetRepository budzetRepository = new BudzetRepository();

        // Prikaz svih zahteva za budzet
        public IActionResult Zahtevi()
        {
            var zahtevi = budzetRepository.GetZahtevBudzetAll();
            return View("Index", zahtevi);
        }
        // Odobri zahtev
        public IActionResult Odobri(int id)
        {
            var zahtev = budzetRepository.GetZahtevBudzetById(id);
            if (zahtev == null) return NotFound();

            budzetRepository.AddToKorisnikBudzet(zahtev.KorisnikId, zahtev.Kolicina);
            budzetRepository.UpdateZahtevBudzetStatus(id, "Odobren");

            // Dodaj obaveštenje za prihvacen zahtev
            budzetRepository.AddObavestenje(zahtev.KorisnikId,
                $"Vaš zahtev za budžet od {zahtev.Kolicina} RSD je odobren.");

            return RedirectToAction("Zahtevi");
        }
        //Odbij zahtev
        public IActionResult Odbij(int id)
        {
            var zahtev = budzetRepository.GetZahtevBudzetById(id);
            if (zahtev == null) return NotFound();

            budzetRepository.UpdateZahtevBudzetStatus(id, "Odbijen");

            // Obaveštenje za odbijen zahtev
            budzetRepository.AddObavestenje(zahtev.KorisnikId,
                $"Vaš zahtev za budžet od {zahtev.Kolicina} RSD je odbijen.");

            return RedirectToAction("Zahtevi");
        }
    }
}
