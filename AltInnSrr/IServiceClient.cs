using System;
using System.Threading.Tasks;
using AltInnSrr.Connected_Services.AltInnSrrService;

namespace AltInnSrr
{
    public interface IServiceClient
    {
        Task<GetRightResponseList> GetAllRights();
        Task<GetRightResponseList> GetRights(int orgnr);
        Task<DeleteRightResponseList> DeleteRights(int orgnr);
        Task<AddRightResponseList> AddRights(int orgnr, DateTime validTo);
        
    }
}