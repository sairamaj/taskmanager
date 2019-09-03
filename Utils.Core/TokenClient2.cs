using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using IdentityModelTokenClient=IdentityModel.Client;

namespace Utils.Core
{
    public class TokenClient : ITokenClient
    {
        public TokenClient(HttpClientHandler clientHandler)
        {
            ClientHandler = clientHandler ?? throw new ArgumentNullException(nameof(clientHandler));
        }

        public HttpClientHandler ClientHandler { get; }

        public async Task<string> GetAccessTokenAsync(string address, string clientId, string clientSecret, object extraData)
        {
            var client = new IdentityModelTokenClient.TokenClient(
                address,
                clientId,
                clientSecret,
                ClientHandler)
            {
                BasicAuthenticationHeaderStyle = BasicAuthenticationHeaderStyle.Rfc2617
            };
            var response = await client.RequestClientCredentialsAsync("identity request_claims", extraData);
            if (response.HttpStatusCode != HttpStatusCode.OK)
            {
                // todo: change 
                throw new Exception($"Could not get access token from: {address} {response.HttpStatusCode} {response.ErrorDescription} {response.Json}");
            }

            return response.AccessToken;
        }
    }
}
