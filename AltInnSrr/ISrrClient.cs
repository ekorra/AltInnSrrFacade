using System;
using System.Threading.Tasks;

namespace AltInnSrr
{
    public interface ISrrClient
    {
        Task<AltInnSrrRights> GetRights(int orgnr);
        Task DeleteRights(int orgnr);
        Task<AltInnSrrRights> UpdateRights(int orgnr, DateTime ValidTo);
        Task<AltInnSrrRights> AddRights(int orgnr);
    }
}
