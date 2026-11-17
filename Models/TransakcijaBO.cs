using System;

namespace PrijavaIspita.Models
{
    public class TransakcijaBO
    {
        public int Id { get; set; }
        public KorisnikBO Korisnik { get; set; }
        public string Tip { get; set; }
        public float Iznos { get; set; }
        public DateTime Datum { get; set; }
    }
}
