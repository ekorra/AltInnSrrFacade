using System.Threading.Tasks;

namespace AltInnSrr.Lib
{
    public interface IEnhetsregisteretClient
    {
        Task<EnhetsregisteretContract> GetEnhetInfo(string orgnr);
    }
}