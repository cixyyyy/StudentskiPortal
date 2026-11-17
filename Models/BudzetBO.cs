namespace PrijavaIspita.Models
{
    public class BudzetBO
    {
        public int Id { get; set; }
        public KorisnikBO Korisnik { get; set; }
        public double Iznos { get; set; }
        public string Status { get; set; } = null!;
    }
}