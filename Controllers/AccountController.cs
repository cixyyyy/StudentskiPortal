using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrijavaIspita.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace PrijavaIspita.Controllers
{
    public class AccountController : Controller
    {
        private readonly PrijavaIspitaContext _context;

        public AccountController(PrijavaIspitaContext context)
        {
            _context = context;
        }

        // Login stranica
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Login - provera korisnika
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var korisnik = await _context.Korisnici
                    .FirstOrDefaultAsync(s => s.Email == model.Email && s.Password == model.Password);

                if (korisnik != null)
                {
                    // uzimamo rolu korisnika iz UserRole tabele
                    var userRole = await _context.UserRoles
                        .Include(ur => ur.Role)
                        .FirstOrDefaultAsync(ur => ur.KorisnikID == korisnik.KorisnikID);

                    string roleName = userRole != null ? userRole.Role.RoleName : "Student";

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, korisnik.Email),
                        new Claim(ClaimTypes.Role, roleName)
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    // 1. SignIn pomoću cookie autentikacije
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    // 2. Ostavljamo sesiju za interne potrebe
                    HttpContext.Session.SetString("Email", korisnik.Email);
                    HttpContext.Session.SetString("Role", roleName);
                    HttpContext.Session.SetInt32("KorisnikID", korisnik.KorisnikID);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Neispravan email ili lozinka.");
            }

            return View(model);
        }

        // Logout metoda
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.Session.Remove("Email");
            HttpContext.Session.Remove("Role");
            HttpContext.Session.Remove("KorisnikID");

            return RedirectToAction("Login");
        }

        // role-based autorizacija
        [Authorize(Roles = "Administrator")]
        public IActionResult AdminOnly()
        {
            return Content("Stranica za administratore.");
        }

        [Authorize(Roles = "Profesor")]
        public IActionResult ProfesorOnly()
        {
            return Content("Stranica za profesore.");
        }

        [Authorize(Roles = "Student")]
        public IActionResult StudentOnly()
        {
            return Content("Stranica za studente.");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            ViewBag.Smerovi = _context.Smer.ToList();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var korisnik = new Korisnik
                {
                    Ime = model.Ime,
                    Prezime = model.Prezime,
                    Email = model.Email,
                    Password = model.Password,
                    SmerId = model.SmerId,
                    DatumRodjenja = model.DatumRodjenja,
                    GodinaUpisa = model.GodinaUpisa
                };

                // Dodaj korisnika u bazu
                _context.Korisnici.Add(korisnik);
                await _context.SaveChangesAsync();

                // Dodaj UserRole automatski - student pri registraciji
                _context.UserRoles.Add(new UserRole
                {
                    KorisnikID = korisnik.KorisnikID,
                    RoleID = 1  // Student
                });
                await _context.SaveChangesAsync();

                // DODAJ BUDZET za novog korisnika
                _context.Budzets.Add(new Budzet
                {
                    KorisnikID = korisnik.KorisnikID,
                    Iznos = 1000,
                    Status = "Aktivan"
                });
                await _context.SaveChangesAsync();

                // odmah uloguj korisnika pri registraciji
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, korisnik.Email),
                    new Claim(ClaimTypes.Role, "Student")
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                // Opcionalno sesija
                HttpContext.Session.SetString("Email", korisnik.Email);
                HttpContext.Session.SetString("Role", "Student");
                HttpContext.Session.SetInt32("KorisnikID", korisnik.KorisnikID);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Smerovi = _context.Smer.ToList();
            return View(model);
        }
    }
}
