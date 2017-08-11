using System;
using System.Threading.Tasks;
using AltInnSrr.Lib.Connected_Services.AltInnSrrService;

namespace AltInnSrr.Lib
{
    public interface IServiceClient
    {
        Task<GetRightResponseList> GetAllRights();
        Task<GetRightResponseList> GetRights(int orgnr);
        Task<DeleteRightResponseList> DeleteRights(int orgnr);
        Task<AddRightResponseList> AddRights(int orgnr, DateTime validTo);
        
    }
}