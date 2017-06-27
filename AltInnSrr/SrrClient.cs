using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AltInnSrr.Lib.Connected_Services.AltInnSrrService;

namespace AltInnSrr.Lib
{

    public class SrrClient : ISrrClient
    {
        private readonly IServiceClient serviceClient;

        public SrrClient(IServiceClient serviceClient)
        {
            this.serviceClient = serviceClient;
        }

        public async Task<IEnumerable<AltInnSrrRights>> GetAllRights()
        {
            var result = await serviceClient.GetAllRights();
            var group = result.GroupBy(g => g.Reportee);

            var srrRightsList = new List<AltInnSrrRights>();
            foreach (var reportee in group)
            {
                srrRightsList.Add(GetAltInnSrrRights(reportee));
            }
           return srrRightsList;
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
            
            if(getRightResponses.Any())
            {
                return new AltInnSrrRights()
                {
                    ReadRightValidTo = getRightResponses.FirstOrDefault(r => r.Right == RegisterSRRRightsType.Read)?.ValidTo ?? DateTime.MinValue,
                    WriteRightValidTo = getRightResponses.FirstOrDefault(r => r.Right == RegisterSRRRightsType.Write)?.ValidTo ?? DateTime.MinValue,
                    OrgNr = int.Parse(getRightResponses.First().Reportee)
                };
            }
            return new AltInnSrrRights();
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
            
            var errorResponses = errors as IList<DeleteRightResponse> ?? errors.ToList();
            if (errorResponses.Any() )
            {
                HandleErrors(errorResponses);
            }
        }

        private static void HandleErrors(IList<DeleteRightResponse> errorResponses)
        {
            var errorlist = errorResponses.ToList();

            var messages = errorResponses.Select(s => $"{s.Right.ToString()} - {s.OperationResult}");
            throw new AltInnSrrException($"Feil ved sletting av rettigheter: {string.Join(", ", messages)}",
                errorlist.Where(o => o.OperationResult != OperationResult.Ok).Select(o => o.OperationResult).ToList());
        }

        private static void HandleErrors(IList<AddRightResponse> errorResponses)
        {
            var errorlist = errorResponses.ToList();

            var messages = errorResponses.Select(s => $"{s.Right.ToString()} - {s.OperationResult}");
            throw new AltInnSrrException($"Feil ved oppretting av rettigheter: {string.Join(", ", messages)}",
                errorlist.Where(o => o.OperationResult != OperationResult.Ok).Select(o => o.OperationResult).ToList());
        }

        public async Task<AltInnSrrRights> UpdateRights(int orgnr, DateTime validTo)
        {
            try
            {
                await DeleteRights(orgnr);
            }
            catch (AltInnSrrException e)
            {
                if (e.AltInnFaultResult.All(r => r != OperationResult.RuleNotFound))
                {
                    throw;
                }
            }
            return await AddRights(orgnr, validTo);
        }

        public async Task<AltInnSrrRights> AddRights(int orgnr)
        {
            return await AddRights(orgnr, DateTime.Now.AddYears(2));
        }

        public async Task<AltInnSrrRights> AddRights(int orgnr, DateTime endDate)
        {
            
            var result = await serviceClient.AddRights(orgnr, endDate);
            var errors = result.Where(r => r.OperationResult != OperationResult.Ok);

            var errorResponses = errors as IList<AddRightResponse> ?? errors.ToList();
            if (errorResponses.Any())
            {
                HandleErrors(errorResponses);
            }
            return GetAddAltInnSrrRights(result);
        }

        private static AltInnSrrRights GetAddAltInnSrrRights(IEnumerable<AddRightResponse> result)
        {
            var getRightResponses = result as AddRightResponse[] ?? result.ToArray();
            var altInnSrrRights = new AltInnSrrRights()
            {
                ReadRightValidTo = getRightResponses.FirstOrDefault(r => r.Right == RegisterSRRRightsType.Read)?.ValidTo ?? DateTime.MinValue,
                WriteRightValidTo = getRightResponses.FirstOrDefault(r => r.Right == RegisterSRRRightsType.Write)?.ValidTo ?? DateTime.MinValue
            };
            return altInnSrrRights;
        }
    }
}
