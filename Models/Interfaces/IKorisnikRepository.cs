using System.Collections.Generic;

namespace PrijavaIspita.Models.EFRepository
{
    public interface IKorisnikRepository
    {
        IEnumerable<KorisnikBO> GetAll();
        KorisnikBO GetById(int id);
        void Add(KorisnikBO korisnik);
        void Update(KorisnikBO korisnik);
        void Delete(int id);
    }
}
