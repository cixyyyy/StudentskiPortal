namespace PrijavaIspita.Models
{
    public class ZahtevBudzet
    {
        public int ZahtevBudzetId { get; set; }
        public int KorisnikId { get; set; }
        public decimal Kolicina { get; set; }
        public DateTime Datum { get; set; }
        public string Status { get; set; } = null!;

        public Korisnik Korisnik { get; set; } = null!;
    }
}
