using System.Collections.Generic;

namespace PrijavaIspita.Models.Interfaces
{
    public interface IPrijavaIspitaRepository
    {
        IEnumerable<PredmetBO> GetAllPredmeti();

        IEnumerable<PrijavaIspitaBO> GetAll();
        IEnumerable<PrijavaIspitaBO> GetByPredmetId(int predmetId);
        IEnumerable<PrijavaIspitaBO> GetByKorisnikId(int korisnikId);

        void Add(PrijavaIspitaBO prijavaIspita);

        IEnumerable<KorisnikBO> GetAllKorisnici();
    }
}
