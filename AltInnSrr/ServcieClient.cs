using System;
using System.ServiceModel;
using System.Threading.Tasks;
using AltInnSrr.Connected_Services.AltInnSrrService;

namespace AltInnSrr
{
    public class ServcieClient: IServiceClient

    {
        public async Task<GetRightResponseList>  GetAllRights()
        {
            var client = new RegisterSRRAgencyExternalBasicClient();
            var result = await client.GetRightsBasicAsync("", "", "", 0, "");
            return result.Body.GetRightsBasicResult;
        }

        public async Task<GetRightResponseList> GetRights(string orgnr)
        {
            var client = new RegisterSRRAgencyExternalBasicClient();
            client.Endpoint.Address = new EndpointAddress("https://www.altinn.no/RegisterExternal/RegisterSRRAgencyExternalBasic.svc");

            
            var basicHttpsBinding = new BasicHttpsBinding();
            client.Endpoint.Binding = basicHttpsBinding;

            try
            {
                var result = await client.GetRightsBasicAsync("", "", "", 0, orgnr);
                return result.Body.GetRightsBasicResult;
            }
            catch (FaultException<AltinnFault> e)
            {
                Console.Write(e);
                throw;
            }
            catch (FaultException e)
            {
                var code = e.Code;
                var message = e.Message;
                Console.WriteLine(e);
                throw;
            }
        }
    }

    public interface IServiceClient
    {
        Task<GetRightResponseList> GetAllRights();
        Task<GetRightResponseList> GetRights(string orgnr);
    }
}
