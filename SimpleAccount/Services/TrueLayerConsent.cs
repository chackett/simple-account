using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.VisualBasic;
using SimpleAccount.Domains;
using SimpleAccount.Repositories;

namespace SimpleAccount.Services
{
    public class TrueLayerConsent : IConsentService
    {
        private readonly IRepository<Consent> _repository;

        private const string TrueLayerTokenEndpoint = "https://auth.truelayer.com/connect/token";
        
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _redirectUri; // Could be a collection to support multiple URLs
        private readonly string _authorisationUrl;

        public TrueLayerConsent(IConfiguration config, IRepository<Consent> repository)
        {
            _repository = repository;
            _clientId = config["clientId"];
            _clientSecret = config["clientSecret"];
            _redirectUri = config["redirectUri"];
            _authorisationUrl = config["authorisationUrl"];
        }

        public string AuthorisationUrl(string userId)
        {
            // Probably a redundant getter.
            return $"{_authorisationUrl}&state={userId}";
        }

        public async Task CallbackAsync(string code, string state)
        {
            var requestBody = new Dictionary<string, string>()
            {
                ["grant_type"] = "authorization_code",
                ["client_id"] = _clientId,
                ["client_secret"] = _clientSecret,
                ["redirect_uri"] = _redirectUri,
                ["code"] = code
            };

            var client = new HttpClient();
            var response = await client.PostAsync(TrueLayerTokenEndpoint, new FormUrlEncodedContent(requestBody));

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("unexpected consent service response");
            }
            var responseBody = await response.Content.ReadAsStringAsync();

            
            using var jsonDoc = JsonDocument.Parse(responseBody);
            var values = jsonDoc.RootElement;
            
            var jwtHandler = new JwtSecurityTokenHandler();
            var token = jwtHandler.ReadJwtToken(values.GetProperty("access_token").GetString());

            if (token == null)
            {
                throw new Exception("unable to retrieve consent");
            }
            var consent = new Consent()
            {
                ConsentId = state,
                AccessTokenRaw = values.GetProperty("access_token").GetString(),
                RefreshTokenRaw = values.GetProperty("refresh_token").GetString(),
                AccessTokenExpiry = token.Claims.First(x => x.Type == "provider_access_token_expiry").Value,
                RefreshTokenExpiry = token.Claims.First(x => x.Type == "provider_refresh_token_expiry").Value,
            };
            
            _repository.Add(consent);
        }

        public Consent GetConsent(string userId)
        {
           return _repository.Get(userId);
        }
    }
}