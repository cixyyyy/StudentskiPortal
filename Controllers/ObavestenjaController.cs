using Microsoft.AspNetCore.Mvc;
using PrijavaIspita.Models.EFRepository;

public class ObavestenjaController : Controller
{
    private readonly PrijavaIspitaRepository _repo;

    public ObavestenjaController()
    {
        _repo = new PrijavaIspitaRepository();
    }

    public IActionResult Index()
    {
        int? studentId = HttpContext.Session.GetInt32("KorisnikID");
        if (studentId == null)
            return RedirectToAction("Login", "Account");

        var obavestenja = _repo.GetObavestenja(studentId.Value);
        return View(obavestenja);
    }
}
