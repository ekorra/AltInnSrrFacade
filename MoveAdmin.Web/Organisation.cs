using System;
using System.Threading.Tasks;
using AltInnSrr;
using Newtonsoft.Json;

namespace MoveAdmin.Web
{
    public class Organisation
    {
        private readonly ISrrClient srrClient;
        public string OrganisationNumber { get; set; }
        public string Name { get; set; }

        public AltInnSrrRights AltInnSrrRights { get; set; }

        public Organisation(ISrrClient srrClient)
        {
            this.srrClient = srrClient;
        }

        public async Task GetInforation(int orgnr)
        {           
            AltInnSrrRights = await srrClient.GetRights(orgnr);
        }
       

        public async Task Create(int orgnr)
        {
            AltInnSrrRights = await srrClient.AddRights(orgnr);
        }

        public async Task Update(int orgnr, DateTime? validTo = null)
        {
            if (!validTo.HasValue)
            {
                validTo = DateTime.Now.AddYears(2);
            }
            AltInnSrrRights = await srrClient.UpdateRights(orgnr, validTo.Value);
        }

        public async Task Delete(int orgnr)
        {
            await srrClient.DeleteRights(orgnr);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public bool IsValidOrgnr(int orgnr)
        {
            if (orgnr.ToString().Length != 9)
                return false;

            return orgnr % 11 == 0;
        }

    }
}
