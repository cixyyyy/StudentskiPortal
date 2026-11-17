using System.ComponentModel.DataAnnotations;

namespace PrijavaIspita.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email je obavezan.")]
        [EmailAddress(ErrorMessage = "Nevažeća email adresa.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Lozinka je obavezna.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Zapamti me")]
        public bool RememberMe { get; set; }
    }
}
