using System;
using System.Collections.Generic;

namespace PrijavaIspita.Models
{
    public partial class Predmet
    {
        public int PredmetId { get; set; }

        public string Naziv { get; set; } = null!;

        public int? ProfesorID { get; set; }
        public Korisnik? Profesor { get; set; }

        public int SmerId { get; set; }
        public Smer Smer { get; set; }

        public virtual ICollection<Prijava> Prijavas { get; set; } = new List<Prijava>();
    }
}
