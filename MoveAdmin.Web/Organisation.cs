using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AltInnSrr.Lib;
using Newtonsoft.Json;

namespace AltInnSrr.Api
{
    public class Organisation
    {
        private readonly ISrrClient srrClient;
        public int OrganisationNumber { get; private set; }
        public string Name { get; set; }

        public AltInnSrrRights AltInnSrrRights { get; set; }

        public Organisation(ISrrClient srrClient, int organisationNumber)
        {
            this.srrClient = srrClient;
            OrganisationNumber = organisationNumber;
        }

        private Organisation() { }

        public static Organisation Create(int orgnr, AltInnSrrRights rights)
        {
            return new Organisation{OrganisationNumber = orgnr, AltInnSrrRights = rights};
        }

        public async Task GetInforation()
        {           
            var result = await srrClient.GetRights(OrganisationNumber);
            AltInnSrrRights = result;
        }
        
        public async Task Add()
        {
            AltInnSrrRights = await srrClient.AddRights(OrganisationNumber);
        }

        public async Task Update(AltInnSrrRights altInnSrrRights)
        {
            DateTime validTo;
            if (altInnSrrRights == null || (altInnSrrRights.ReadRightValidTo == DateTime.MinValue || altInnSrrRights.ReadRightValidTo.Date <= DateTime.Now.Date))
            {
                validTo = DateTime.Now.AddYears(2);
            }
            else
            {
                validTo = altInnSrrRights.ReadRightValidTo;
            }
            AltInnSrrRights = await srrClient.UpdateRights(OrganisationNumber, validTo);
        }

        public static async Task<IEnumerable<Organisation>> GetOrganisations(ISrrClient srrClient)
        {
            var organisations = new List<Organisation>();
            var result =  await srrClient.GetAllRights();
            foreach (var srrRights in result)
            {
                var org = new Organisation
                {
                    OrganisationNumber = srrRights.OrgNr,
                    AltInnSrrRights = srrRights
                };
                organisations.Add(org);
            }
            return organisations;
        }
        
        public async Task Delete(int orgnr)
        {
            await srrClient.DeleteRights(orgnr);
            AltInnSrrRights = new AltInnSrrRights();
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
