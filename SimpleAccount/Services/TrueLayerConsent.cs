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
        private readonly IRepository<Consent, string> _repository;
        private readonly ITrueLayerDataApi _dataApi;
        
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _redirectUri; // Could be a collection to support multiple URLs
        private readonly string _authorisationUrl;

        public TrueLayerConsent(IConfiguration config, IRepository<Consent, string> repository, ITrueLayerDataApi dataApi)
        {
            _repository = repository;
            _dataApi = dataApi;

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
            var tlToken = await _dataApi.GetAccessToken(code, state);
            
            var jwtHandler = new JwtSecurityTokenHandler();
            var atJwt = jwtHandler.ReadJwtToken(tlToken.AccessToken);
            
            if (atJwt == null)
            {
                throw new Exception("unable to retrieve consent");
            }
            
            var consent = new Consent()
            {
                ConsentId = state,
                AccessTokenRaw = tlToken.AccessToken,
                RefreshTokenRaw = tlToken.RefreshToken,
                AccessTokenExpiry = atJwt.Claims.First(x => x.Type == "provider_access_token_expiry").Value,
                RefreshTokenExpiry = atJwt.Claims.First(x => x.Type == "provider_refresh_token_expiry").Value,
            };
            
            _repository.Add(consent);
        }

        public Consent GetConsent(string userId)
        {
           return _repository.Get(userId);
        }
    }
}