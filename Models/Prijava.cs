using PrijavaIspita.Models;

public partial class Prijava
{
    public int PredmetId { get; set; }
    public int KorisnikID { get; set; }
    public int Ocena { get; set; }

    public string Rok { get; set; } = null!;
    public DateTime DatumPrijave { get; set; }

    public virtual Predmet Predmet { get; set; } = null!;
    public virtual Korisnik Korisnik { get; set; } = null!;
}