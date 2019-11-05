using System.Threading.Tasks;

namespace Utils.Core
{
    /// <summary>
    /// Token client for getting tokens.
    /// </summary>
    public interface ITokenClient
    {
        /// <summary>
        /// Gets access token.
        /// </summary>
        /// <param name="address">
        /// Address of the service.
        /// </param>
        /// <param name="clientId">
        /// Client id.
        /// </param>
        /// <param name="clientSecret">
        /// Client secret.
        /// </param>
        /// <param name="extraData">
        /// Additional data to be sent.
        /// </param>
        /// <returns>
        /// An access token.
        /// </returns>
        Task<string> GetAccessTokenAsync(string address, string clientId, string clientSecret, object extraData);
    }
}
