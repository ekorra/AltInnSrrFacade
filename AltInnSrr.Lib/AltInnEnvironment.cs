namespace AltInnSrr.Lib
{
    public class AltInnEnvironment
    {
        public string AltInnUserName { get; set; }
        public string AltInnPassword { get; set; }
        public string ServiceCode { get; set; }
        public int ServiceEditionCode { get; set; }
        public string EndpointUri { get; set; }

        public AltInnEnvironment()
        {
        }
    }
}
