using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AltInnSrr.Lib
{
    public interface ISrrClient
    {
        Task<AltInnSrrRights> GetRights(int orgnr);
        Task DeleteRights(int orgnr);
        Task<AltInnSrrRights> UpdateRights(int orgnr, DateTime ValidTo);
        Task<AltInnSrrRights> AddRights(int orgnr);
        Task<IEnumerable<AltInnSrrRights>> GetAllRights();
    }
}
