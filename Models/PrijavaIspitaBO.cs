using System;
using System.ComponentModel.DataAnnotations;

namespace PrijavaIspita.Models
{
    public class PrijavaIspitaBO
    {
        public int Id { get; set; }

        public int Ocena { get; set; }

        [Required(ErrorMessage = "Predmet je obavezan")]
        public PredmetBO Predmet { get; set; } = null!;

        [Required(ErrorMessage = "Korisnik je obavezan")]
        public KorisnikBO Korisnik { get; set; } = null!;

        [Required(ErrorMessage = "Rok je obavezan")]
        public string Rok { get; set; } = null!;

        public DateTime DatumPrijave { get; set; } = DateTime.Now;
    }
}
