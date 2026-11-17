using PrijavaIspita.Models;

public partial class Budzet
{
    public int BudzetId { get; set; }

    public int KorisnikID { get; set; }

    public double Iznos { get; set; }
    public string Status { get; set; } = null!;

    public virtual Korisnik Korisnik { get; set; } = null!;
}
