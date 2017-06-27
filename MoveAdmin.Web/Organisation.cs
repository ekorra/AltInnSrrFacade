using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AltInnSrr.Lib;
using Newtonsoft.Json;

namespace AltInnSrr.Api
{
    public class Organisation
    {
        private readonly ISrrClient srrClient;
        private readonly IEnhetsregisteretClient enhetsregisteretClient;

        public int OrganisationNumber { get; private set; }
        public string Name { get; set; }
        public EnhetsregisteretContract EnhetsregisteretInfo { get; private set; }

        public AltInnSrrRights AltInnSrrRights { get; set; }

        public Organisation(int organisationNumber, ISrrClient srrClient, IEnhetsregisteretClient enhetsregisteretClient)
        {
            this.srrClient = srrClient;
            OrganisationNumber = organisationNumber;
            this.enhetsregisteretClient = enhetsregisteretClient;
        }

        private Organisation() { }

        public async Task GetInforation()
        {
            EnhetsregisteretInfo = await enhetsregisteretClient.GetEnhetInfo(OrganisationNumber.ToString());
            AltInnSrrRights = await srrClient.GetRights(OrganisationNumber); 
        }
        
        public async Task Add()
        {
            EnhetsregisteretInfo = await enhetsregisteretClient.GetEnhetInfo(OrganisationNumber.ToString());
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
                //var enhetsinfo = enhetsregisteretClient.GetEnhetInfo()
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
