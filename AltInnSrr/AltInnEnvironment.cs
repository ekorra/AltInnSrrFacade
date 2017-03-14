using System.ServiceModel;

namespace AltInnSrr
{
    public class AltInnEnvironment
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ServiceCode { get; set; }
        public int ServiceEditionCode { get; set; }
        public string EndpointUri { get; set; }

        public AltInnEnvironment()
        {
            
        }


        
    }
}
