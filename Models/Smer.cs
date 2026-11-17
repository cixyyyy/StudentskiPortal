using System;
using System.Collections.Generic;

namespace PrijavaIspita.Models;

public partial class Smer
{
    public int SmerId { get; set; }

    public string Naziv { get; set; } = null!;

    public virtual ICollection<Korisnik> Korisnici { get; set; } = new List<Korisnik>();

}
