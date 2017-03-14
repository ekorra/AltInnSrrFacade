using System.ServiceModel;
using System.ServiceModel.Channels;

namespace AltInnTilgangsstyring.AltInn
{
    public class AltInnEnvironment
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ServiceCode { get; set; }
        public int ServiceEditionCode { get; set; }
        public EndpointAddress EndpointAddress { get; set; }

        public AltInnEnvironment(string userName, string password, string serviceCode, int serviceEditionCode, EndpointAddress endpointAddress)
        {
            UserName = userName;
            Password = password;
            ServiceCode = serviceCode;
            ServiceEditionCode = serviceEditionCode;
            EndpointAddress = endpointAddress;
        }

        
    }
}
