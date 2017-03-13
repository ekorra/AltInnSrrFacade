using System.ServiceModel;

namespace AltInnTilgangsstyring.AltInn
{
    public class AltInnEnvironment
    {
        public string User { get; set; }
        public string Password { get; set; }
        public string ServiceCode { get; set; }
        public int ServiceEditionCode { get; set; }
        public EndpointAddress EndpointAddress { get; set; }

        public AltInnEnvironment(string user, string password, string serviceCode, int serviceEditionCode, EndpointAddress endpointAddress)
        {
            User = user;
            Password = password;
            ServiceCode = serviceCode;
            ServiceEditionCode = serviceEditionCode;
            EndpointAddress = endpointAddress;
        }
    }
}
