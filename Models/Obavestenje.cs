namespace PrijavaIspita.Models
{
    public class Obavestenje
    {
        public int ObavestenjeId { get; set; }

        public int KorisnikID { get; set; }

        public string Tekst { get; set; }

        public DateTime Datum { get; set; }

        public virtual Korisnik Korisnik { get; set; } = null!;
    }
}