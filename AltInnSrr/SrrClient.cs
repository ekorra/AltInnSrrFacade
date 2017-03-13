using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AltInnSrr.Connected_Services.AltInnSrrService;
using MoveAdmin.Commons;

namespace AltInnSrr
{

    public class SrrClient : ISrrClient
    {
        private readonly IServiceClient serviceClient;

        public SrrClient(IServiceClient serviceClient)
        {
            this.serviceClient = serviceClient;
        }

        public async Task<AltInnSrrRights> GetRights(int orgnr)
        {
            var result = await serviceClient.GetRights(orgnr.ToString());
            var altInnSrrRights = new AltInnSrrRights()
            {
                ReadRightValidTo = GetValidToDate(result, RegisterSRRRightsType.Read),
                WriteRightValidTo = GetValidToDate(result, RegisterSRRRightsType.Write)
            };
            return altInnSrrRights;
        }

        private static DateTime GetValidToDate(GetRightResponseList result, RegisterSRRRightsType type)
        {
            var right = result.FirstOrDefault(r => r.Right == type);
            if (right != null)
            {
                return right.ValidTo;
            }
            return DateTime.MinValue;

        }

        public async Task<IEnumerable<AltInnSrrRights>> GetRights()
        {
            throw new NotImplementedException();
        }

        public async Task DeleteRights(int orgnr)
        {
            throw new NotImplementedException();
        }

        public async Task<AltInnSrrRights> UpdateRights(int orgnr, DateTime ValidTo)
        {
            throw new NotImplementedException();
        }

        public async Task<AltInnSrrRights> AddRights(int orgnr)
        {
            throw new NotImplementedException();
        }
    }
}
