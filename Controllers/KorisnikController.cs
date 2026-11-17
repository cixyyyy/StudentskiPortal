using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrijavaIspita.Models;
using PrijavaIspita.Models.EFRepository;

namespace PrijavaIspita.Controllers
{
    [Authorize(Roles = "Student")]
    public class KorisnikController : Controller
    {
        // Instanca repository-ja
        KorisnikRepository studentRepository = new KorisnikRepository();

        // Index prikazuje sve studente
        public IActionResult Index()
        {
            return View(studentRepository.GetAll());
        }

        // Create GET - prikazuje formu za dodavanje
        public IActionResult Create()
        {
            return View();
        }

        // Create POST - dodavanje novog studenta
        [HttpPost]
        public IActionResult Create(KorisnikBO korisnik)
        {
            if (!ModelState.IsValid)
            {
                return View(korisnik);
            }

            studentRepository.Add(korisnik);
            return RedirectToAction("Index");
        }

        // Edit GET - prikazuje formu za izmenu
        public IActionResult Edit(int id)
        {
            var student = studentRepository.GetById(id);
            if (student == null)
                return NotFound();

            return View(student);
        }

        // Edit POST - čuvanje izmena
        [HttpPost]
        public IActionResult Edit(KorisnikBO korisnik)
        {
            if (!ModelState.IsValid)
                return View(korisnik);

            studentRepository.Update(korisnik);
            return RedirectToAction("Index");
        }

        // Delete GET - potvrda brisanja
        public IActionResult Delete(int id)
        {
            var student = studentRepository.GetById(id);
            if (student == null)
                return NotFound();

            return View(student);
        }

        // Delete POST - brisanje studenta
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            studentRepository.Delete(id);
            return RedirectToAction("Index");
        }

        //Metoda za obavestenja
        public IActionResult Obavestenja(int studentId)
        {
            var studentRepo = new KorisnikRepository();
            var student = studentRepo.GetById(studentId);
            if (student == null) return NotFound();

            return View(student.Obavestenja);
        }
    }
}
