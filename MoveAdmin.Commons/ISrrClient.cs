using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoveAdmin.Commons
{
    public interface ISrrClient
    {
        Task<AltInnSrrRights> GetRights(int orgnr);
        Task<IEnumerable<AltInnSrrRights>> GetRights();
        Task DeleteRights(int orgnr);
        Task<AltInnSrrRights> UpdateRights(int orgnr, DateTime ValidTo);
        Task<AltInnSrrRights> AddRights(int orgnr);
    }
}
