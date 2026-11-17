using System.ComponentModel.DataAnnotations;

namespace PrijavaIspita.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Unesite ime")]
        public string Ime { get; set; } = null!;

        [Required(ErrorMessage = "Unesite prezime")]
        public string Prezime { get; set; } = null!;

        [Required(ErrorMessage = "Unesite email")]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Unesite lozinku")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Potvrdite lozinku")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Lozinke se ne poklapaju")]
        public string ConfirmPassword { get; set; } = null!;

        [Required(ErrorMessage = "Odaberite smer")]
        public int SmerId { get; set; }

        [Required(ErrorMessage = "Unesite datum rodjenja")]
        [DataType(DataType.Date)]
        public DateTime DatumRodjenja { get; set; }

        [Required(ErrorMessage = "Unesite godinu upisa")]
        [Range(2017, 2025, ErrorMessage = "Unesite godinu upisa")]
        public short GodinaUpisa { get; set; }
    }
}