using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AltInnSrr.Lib
{
    public class EnhetsregisteretClient : IEnhetsregisteretClient
    {
        private readonly HttpClient httpClient;
        const string BrregBaseAddress = "http://data.brreg.no/";

        public EnhetsregisteretClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            httpClient.BaseAddress = new Uri(BrregBaseAddress);
        }

        public async Task<EnhetsregisteretContract> GetEnhetInfo(string orgnr)
        {
            var enhet = await GetEnhet(orgnr, OrganisationType.Enhet);
            if (enhet != null) return enhet;

            var underenhet = await GetEnhet(orgnr, OrganisationType.Underenhet);
            if (underenhet != null) return underenhet;

            throw new EnhetNotFoundException($"Enhet med orgnr {orgnr} ble ikke funnet i enhetsregisteret");
        }

        private async Task<EnhetsregisteretContract> GetEnhet(string orgnr, OrganisationType organisationType)
        {
            try
            {
                var responseMessage = await httpClient.GetAsync($"enhetsregisteret/{organisationType.ToString().ToLower()}/{orgnr}.json");

                if (responseMessage.IsSuccessStatusCode)
                {
                    var result = await responseMessage.Content.ReadAsStringAsync();
                    var enhetsregisterContract = JsonConvert.DeserializeObject<EnhetsregisteretContract>(result);


                    return enhetsregisterContract;
                }
                if (responseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                throw new Exception($"Unexcpected http response from Brreg: {responseMessage.StatusCode}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public enum OrganisationType
        {
            Enhet,
            Underenhet
        }
    }
}
