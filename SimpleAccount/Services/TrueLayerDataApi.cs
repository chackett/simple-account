using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SimpleAccount.DTO.Response;

namespace SimpleAccount.Services
{
    public class ConcreteTrueLayerDataApi : ITrueLayerDataApi
    {
        private const string BaseUrl = "https://api.truelayer.com/data/v1";
        private const string TrueLayerTokenEndpoint = "https://auth.truelayer.com/connect/token";
        
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _redirectUri;
        private readonly string _authorisationUrl;

        public ConcreteTrueLayerDataApi(IConfiguration config)
        {
            _clientId = config["clientId"];
            _clientSecret = config["clientSecret"];
            _redirectUri = config["redirectUri"];
            _authorisationUrl = config["authorisationUrl"];
        }
        
        public string AuthorisationUrl(string state)
        {
            return $"{_authorisationUrl}&state={state}";
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
                throw new Exception("unexpected service response");
            }
            var responseBody = await response.Content.ReadAsStringAsync();
            
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            
            return JsonSerializer.Deserialize<TrueLayerAccessToken>(responseBody, options);
        }

        public async Task<List<Account>> GetAccounts(string accessToken)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            
            var response = await client.GetAsync($"{BaseUrl}/accounts");

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("unexpected service response");
            }
            var responseBody = await response.Content.ReadAsStringAsync();
            
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            
            var resp = JsonSerializer.Deserialize<AccountList>(responseBody, options);

            if (resp.Status != "Succeeded" || resp.Accounts == null)
            {
                throw new Exception("failed to retrieve accounts");
            }

            return resp.Accounts.ToList();
        }

        public Task<Account> GetAccount(string accessToken, string accountId)
        {
            throw new System.NotImplementedException();
        }

        public List<Transaction> GetTransactions(string accessToken)
        {
            throw new System.NotImplementedException();
        }
    }
}