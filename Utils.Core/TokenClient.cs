using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using IdentityModelTokenClient=IdentityModel.Client;

namespace Utils.Core
{
    /// <summary>
    /// Token client to get access token.
    /// </summary>
    public class TokenClient : ITokenClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TokenClient"/> class.
        /// </summary>
        /// <param name="clientHandler">
        /// A <see cref="HttpClientHandler"/> instance.
        /// </param>
        public TokenClient(HttpClientHandler clientHandler)
        {
            this.ClientHandler = clientHandler ?? throw new ArgumentNullException(nameof(clientHandler));
        }

        /// <summary>
        /// Gets client handler.
        /// </summary>
        public HttpClientHandler ClientHandler { get; }

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
        public async Task<string> GetAccessTokenAsync(string address, string clientId, string clientSecret, object extraData)
        {
#pragma warning disable 618
            var client = new IdentityModelTokenClient.TokenClient(
#pragma warning restore 618
                address,
                clientId,
                clientSecret,
                this.ClientHandler)
            {
                BasicAuthenticationHeaderStyle = BasicAuthenticationHeaderStyle.Rfc2617,
            };

            var response = await client.RequestClientCredentialsAsync("identity request_claims", extraData);
            if (response.HttpStatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Could not get access token from: {address} {response.HttpStatusCode} {response.ErrorDescription} {response.Json}");
            }

            return response.AccessToken;
        }
    }
}
