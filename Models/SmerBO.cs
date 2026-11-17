using System.ComponentModel.DataAnnotations;

namespace PrijavaIspita.Models
{
    public class SmerBO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv smera je obavezan")]
        public string Naziv { get; set; } = null!;

        public override string ToString()
        {
            return Naziv;
        }
    }
}
