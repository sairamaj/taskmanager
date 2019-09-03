using System.Threading.Tasks;

namespace Utils.Core
{
    public interface ITokenClient
    {
        Task<string> GetAccessTokenAsync(string address, string clientId, string clientSecret, object extraData);
    }
}
