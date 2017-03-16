using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AltInnSrr.Connected_Services.AltInnSrrService;

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
            var result = await serviceClient.GetRights(orgnr);
            var altInnSrrRights = GetAltInnSrrRights(result);
            return altInnSrrRights;
        }

        private static AltInnSrrRights GetAltInnSrrRights(IEnumerable<GetRightResponse> result)
        {
            var getRightResponses = result as GetRightResponse[] ?? result.ToArray();
            var altInnSrrRights = new AltInnSrrRights()
            {
                OrgNr = int.Parse( getRightResponses.FirstOrDefault().Reportee),
                ReadRightValidTo = getRightResponses.FirstOrDefault(r => r.Right == RegisterSRRRightsType.Read)?.ValidTo ?? DateTime.MinValue,
                WriteRightValidTo = getRightResponses.FirstOrDefault(r => r.Right == RegisterSRRRightsType.Write)?.ValidTo ?? DateTime.MinValue
            };
            return altInnSrrRights;
        }

       
        public async Task<IEnumerable<AltInnSrrRights>> GetRights()
        {
            var result = await serviceClient.GetAllRights();
            var altinnRights = new List<AltInnSrrRights>();
            var orglist = result.GroupBy(g => g.Reportee).Distinct();
            foreach (var org in orglist)
            {
                var list = result.Where(o => o.Reportee == org.Key.ToString());
                altinnRights.Add(GetAltInnSrrRights(list));
            }
            return altinnRights;
        }

        public async Task DeleteRights(int orgnr)
        {
            var result = await serviceClient.DeleteRights(orgnr);
            var errors = result.Where(r => r.OperationResult != OperationResult.Ok);

            var deleteRightResponses = errors as DeleteRightResponse[] ?? errors.ToArray();
            if (deleteRightResponses.Any() )
            {
                var messages = deleteRightResponses.Select(s => $"{s.Right.ToString()} - {s.OperationResult}");
                throw new AltInnSrrException($"Feil ved sletting av rettigheter: { string.Join(", ", messages)}");
            }
        }

        public async Task<AltInnSrrRights> UpdateRights(int orgnr, DateTime validTo)
        {
            await serviceClient.DeleteRights(orgnr);
            return await AddRights(orgnr, validTo);
        }

        public async Task<AltInnSrrRights> AddRights(int orgnr)
        {
            return await AddRights(orgnr, DateTime.Now.AddYears(2));
        }

        public async Task<AltInnSrrRights> AddRights(int orgnr, DateTime endDate)
        {
            var result = await serviceClient.AddRights(orgnr, endDate);
            return GetAddAltInnSrrRights(result);
        }

        private static AltInnSrrRights GetAddAltInnSrrRights(IEnumerable<AddRightResponse> result)
        {
            var getRightResponses = result as AddRightResponse[] ?? result.ToArray();
            var altInnSrrRights = new AltInnSrrRights()
            {
                OrgNr = int.Parse(getRightResponses.FirstOrDefault().Reportee),
                ReadRightValidTo = getRightResponses.FirstOrDefault(r => r.Right == RegisterSRRRightsType.Read)?.ValidTo ?? DateTime.MinValue,
                WriteRightValidTo = getRightResponses.FirstOrDefault(r => r.Right == RegisterSRRRightsType.Write)?.ValidTo ?? DateTime.MinValue
            };
            return altInnSrrRights;
        }
    }
}
