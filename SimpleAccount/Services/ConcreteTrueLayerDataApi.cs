using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SimpleAccount.Domains;
using Transaction = System.Transactions.Transaction;

namespace SimpleAccount.Services
{
    public class ConcreteTrueLayerDataApi : ITrueLayerDataApi
    {
        private const string TrueLayerTokenEndpoint = "https://auth.truelayer.com/connect/token";
        
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _redirectUri;

        public ConcreteTrueLayerDataApi(IConfiguration config)
        {
            _clientId = config["clientId"];
            _clientSecret = config["clientSecret"];
            _redirectUri = config["redirectUri"];
        }

        public async Task<TrueLayerAccessToken> GetAccessToken(string oneTimeCode, string state)
        {
            var requestBody = new Dictionary<string, string>()
            {
                ["grant_type"] = "authorization_code",
                ["client_id"] = _clientId,
                ["client_secret"] = _clientSecret,
                ["redirect_uri"] = _redirectUri,
                ["code"] = oneTimeCode
            };

            var client = new HttpClient();
            var response = await client.PostAsync(TrueLayerTokenEndpoint, new FormUrlEncodedContent(requestBody));

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("unexpected consent service response");
            }
            var responseBody = await response.Content.ReadAsStringAsync();

            
            // using var jsonDoc = JsonDocument.Parse(responseBody);
            // var values = jsonDoc.RootElement;
            //
            // var jwtHandler = new JwtSecurityTokenHandler();
            // var token = jwtHandler.ReadJwtToken(values.GetProperty("access_token").GetString());
            //
            // if (token == null)
            // {
            //     throw new Exception("unable to retrieve consent");
            // }
            
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            
            return JsonSerializer.Deserialize<TrueLayerAccessToken>(responseBody, options);
        }

        public List<Account> GetAccounts(string accessToken)
        {
            throw new System.NotImplementedException();
        }

        public Account GetAccount(string accessToken, string accountId)
        {
            throw new System.NotImplementedException();
        }

        public List<Transaction> GetTransactions(string accessToken)
        {
            throw new System.NotImplementedException();
        }
    }
}