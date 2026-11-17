using System.Collections.Generic;

namespace PrijavaIspita.Models.Interfaces
{
    public interface IPredmetRepository
    {
        IEnumerable<PredmetBO> GetAllUnused();

        void Delete(int[] predmetiIds);
    }
}
