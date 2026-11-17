using System;
using System.Collections.Generic;

namespace PrijavaIspita.Models
{
    public partial class Korisnik
    {
        public int KorisnikID { get; set; }

        public string Ime { get; set; } = null!;
        public string Prezime { get; set; } = null!;
        public short GodinaUpisa { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public double Skolarina { get; set; }
        public double? Prosek { get; set; }
        public int SmerId { get; set; }
        public double Budzet { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public virtual Smer Smer { get; set; } = null!;

        public virtual ICollection<Budzet> Budzets { get; set; } = new List<Budzet>();
        public virtual ICollection<Prijava> Prijavas { get; set; } = new List<Prijava>();
        public virtual ICollection<Transakcija> Transakcijas { get; set; } = new List<Transakcija>();
        public virtual ICollection<Obavestenje> Obavestenja { get; set; } = new List<Obavestenje>();
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
