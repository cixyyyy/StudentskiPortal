using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PrijavaIspita.Models;
using PrijavaIspita.Models.EFRepository;

namespace PrijavaIspita.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class PredmetController : Controller
    {
        private PredmetRepository predmetRepository = new PredmetRepository();

        // Prikaz svih predmeta
        public IActionResult Index()
        {
            return View(predmetRepository.GetAll());
        }

        // GET - Create
        public IActionResult Create()
        {
            // Popuni comboboxove
            ViewBag.Smerovi = predmetRepository.GetAllSmerovi()
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Naziv })
                .ToList();

            ViewBag.Profesori = predmetRepository.GetAllProfesori()
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.ImePrezime })
                .ToList();

            return View(new PredmetBO());
        }

        // POST - Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PredmetBO model)
        {
            if (!ModelState.IsValid)
            {
                // Ponovo popuni comboboxove
                ViewBag.Smerovi = predmetRepository.GetAllSmerovi()
                    .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Naziv })
                    .ToList();

                ViewBag.Profesori = predmetRepository.GetAllProfesori()
                    .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.ImePrezime })
                    .ToList();

                // Ispisi greške
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                .Select(e => e.ErrorMessage)
                                .ToList();
                ViewBag.Errors = errors;

                return View(model);
            }

            // Dohvati profesora iz baze
            using (var context = new PrijavaIspitaContext())
            {
                var profesor = context.Korisnici.FirstOrDefault(k => k.KorisnikID == model.ProfesorID);
                if (profesor == null)
                {
                    ModelState.AddModelError("ProfesorID", "Odabrani profesor ne postoji");
                    ViewBag.Smerovi = predmetRepository.GetAllSmerovi()
                        .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Naziv })
                        .ToList();
                    ViewBag.Profesori = predmetRepository.GetAllProfesori()
                        .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.ImePrezime })
                        .ToList();
                    return View(model);
                }

                var predmet = new Predmet
                {
                    Naziv = model.Naziv,
                    SmerId = model.SmerId,
                    ProfesorID = profesor.KorisnikID
                };

                context.Predmet.Add(predmet);
                context.SaveChanges();
            }

            TempData["SuccessMessage"] = "Predmet je uspešno dodat.";
            return RedirectToAction("Index");
        }



        // GET - Edit
        public IActionResult Edit(int id)
        {
            var predmet = predmetRepository.GetById(id);
            if (predmet == null)
                return NotFound();

            ViewBag.Smerovi = predmetRepository.GetAllSmerovi()
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Naziv })
                .ToList();

            ViewBag.Profesori = predmetRepository.GetAllProfesori()
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.ImePrezime })
                .ToList();

            return View(predmet);
        }

        // POST - Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PredmetBO model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Smerovi = predmetRepository.GetAllSmerovi()
                    .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Naziv })
                    .ToList();

                ViewBag.Profesori = predmetRepository.GetAllProfesori()
                    .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.ImePrezime })
                    .ToList();

                return View(model);
            }

            predmetRepository.Update(model);
            TempData["SuccessMessage"] = "Predmet je uspešno izmenjen.";
            return RedirectToAction("Index");
        }



        // Brisanje selektovanih predmeta
        [HttpPost]
        public IActionResult DeletePredmete(int[] predmeti)
        {
            predmetRepository.Delete(predmeti);
            return RedirectToAction("Index");
        }
    }
}
