using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PrijavaIspita.Models
{
    public class KorisnikBO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ime je obavezno")]
        public string Ime { get; set; } = null!;

        [Required(ErrorMessage = "Prezime je obavezno")]
        public string Prezime { get; set; } = null!;

        public int GodinaUpisa { get; set; }
        public DateTime DatumRodjenja { get; set; }

        [Required(ErrorMessage = "Smer je obavezan")]
        public SmerBO Smer { get; set; } = null!;

        public string ImePrezime
        {
            get { return Ime + " " + Prezime; }
        }
        public string Email { get; set; } = null!;

        public List<ObavestenjeBO> Obavestenja { get; set; } = new List<ObavestenjeBO>();

        public List<string> RoleNames { get; set; } = new List<string>();
    }
}
