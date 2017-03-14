using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using AltInnSrr.Connected_Services.AltInnSrrService;
using AltInnTilgangsstyring.AltInn;

namespace AltInnSrr
{
    public class ServcieClient: IServiceClient
    {
        private readonly AltInnEnvironment altInnEnvironment;

        public ServcieClient(AltInnEnvironment altInnEnvironment)
        {
            this.altInnEnvironment = altInnEnvironment;
        }

        public async Task<GetRightResponseList>  GetAllRights()
        {
            var client = new RegisterSRRAgencyExternalBasicClient();
            var result = await client.GetRightsBasicAsync(altInnEnvironment.UserName, altInnEnvironment.Password, altInnEnvironment.ServiceCode, altInnEnvironment.ServiceEditionCode,"");
            return result.Body.GetRightsBasicResult;
        }

        public async Task<GetRightResponseList> GetRights(int orgnr)
        {
            var client = GetClient();

            try
            {
                var result = await client.GetRightsBasicAsync(altInnEnvironment.UserName, altInnEnvironment.Password,
                    altInnEnvironment.ServiceCode, altInnEnvironment.ServiceEditionCode, orgnr.ToString());
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
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<DeleteRightResponseList> DeleteRights(int orgnr)
        {
            var client = GetClient();

            try
            {
                var deleteRightRequestList = new DeleteRightRequestList{
                    new DeleteRightRequest{
                        Reportee = orgnr.ToString(),
                        Right =  RegisterSRRRightsType.Read
                    },
                    new DeleteRightRequest
                    {
                        Reportee = orgnr.ToString(),
                        Right = RegisterSRRRightsType.Write
                    }
                };

                var result = await client.DeleteRightsBasicAsync(altInnEnvironment.UserName, altInnEnvironment.Password,
                    altInnEnvironment.ServiceCode, altInnEnvironment.ServiceEditionCode, deleteRightRequestList);
                return result.Body.DeleteRightsBasicResult;
            }
            catch (FaultException<AltinnFault> e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task<AddRightResponseList> AddRights(int orgnr, DateTime validTo)
        {
            var client = GetClient();

            try
            {
                var addRightRequestList = new AddRightRequestList
                {
                    new AddRightRequest
                    {
                        Reportee = orgnr.ToString(),
                        Right = RegisterSRRRightsType.Read,
                        ValidTo = validTo
                    },
                    new AddRightRequest
                    {
                        Reportee = orgnr.ToString(),
                        Right = RegisterSRRRightsType.Write,
                        ValidTo = validTo
                    }
                };

                var result = await client.AddRightsBasicAsync(altInnEnvironment.UserName, altInnEnvironment.Password,
                    altInnEnvironment.ServiceCode, altInnEnvironment.ServiceEditionCode, addRightRequestList);
                return result.Body.AddRightsBasicResult;
            }
            catch (FaultException<AltinnFault> e)
            {
                Console.WriteLine();
                throw;
            }
        }

        private RegisterSRRAgencyExternalBasicClient GetClient()
        {
            var client = new RegisterSRRAgencyExternalBasicClient();
            client.Endpoint.Address = altInnEnvironment.EndpointAddress;
            client.Endpoint.Binding = GetBinding();
            return client;
        }

        private Binding GetBinding()
        {
            var scheme = altInnEnvironment.EndpointAddress.Uri.Scheme;
            return scheme == "https" ? (Binding) new BasicHttpsBinding() : new BasicHttpBinding();
        }
    }


    

    public interface IServiceClient
    {
        Task<GetRightResponseList> GetAllRights();
        Task<GetRightResponseList> GetRights(int orgnr);
        Task<DeleteRightResponseList> DeleteRights(int orgnr);
        Task<AddRightResponseList> AddRights(int orgnr, DateTime validTo);
        
    }
}
