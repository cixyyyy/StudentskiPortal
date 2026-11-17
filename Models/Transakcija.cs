using System;
using System.Collections.Generic;

namespace PrijavaIspita.Models;

public partial class Transakcija
{
    public int TransakcijaId { get; set; }

    public int KorisnikId { get; set; }

    public string Tip { get; set; } = null!;

    public double Iznos { get; set; }

    public DateTime Datum { get; set; }

    public virtual Korisnik Korisnik { get; set; } = null!;
}
