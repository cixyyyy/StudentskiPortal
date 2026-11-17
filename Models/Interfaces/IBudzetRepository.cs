using System.Collections.Generic;

namespace PrijavaIspita.Models.EFRepository
{
    public interface IBudzetRepository
    {
        IEnumerable<BudzetBO> GetAll();
        BudzetBO GetByKorisnikId(int korisnikId);
        BudzetBO GetById(int id);
        void Add(BudzetBO budzet);
        void Update(BudzetBO budzet);
        void Delete(int budzetId);
    }
}
