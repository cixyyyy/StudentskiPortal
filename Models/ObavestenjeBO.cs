namespace PrijavaIspita.Models
{
    public class ObavestenjeBO
    {
        public int Id { get; set; }

        public int KorisnikID { get; set; }

        public string Tekst { get; set; }

        public DateTime Datum { get; set; }
    }
}