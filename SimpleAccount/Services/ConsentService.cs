using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SimpleAccount.Domains;
using SimpleAccount.Repositories;

namespace SimpleAccount.Services
{
    public class ConsentService : IConsentService
    {
        private readonly IRepository<Consent, string> _repository;
        private readonly ITrueLayerDataApi _trueLayerDataApi;

        public ConsentService(IConfiguration config, IRepository<Consent, string> repository,
            ITrueLayerDataApi trueLayerDataApi)
        {
            _repository = repository;
            _trueLayerDataApi = trueLayerDataApi;
        }

        public string AuthorisationUrl(string state)
        {
            return _trueLayerDataApi.AuthorisationUrl(state);
        }

        public async Task<Consent> CallbackAsync(string code, string state)
        {
            var tlToken = await _trueLayerDataApi.GetAccessTokenAsync(code, state);

            var jwtHandler = new JwtSecurityTokenHandler();
            var atJwt = jwtHandler.ReadJwtToken(tlToken.AccessToken);

            if (atJwt == null) throw new Exception("unable to retrieve consent");

            var consent = new Consent
            {
                ConsentId = state,
                AccessTokenRaw = tlToken.AccessToken,
                RefreshTokenRaw = tlToken.RefreshToken,
                // Commented - These seem to be missing from the Mock Back response.
                // AccessTokenExpiry = atJwt.Claims.First(x => x.Type == "provider_access_token_expiry").Value,
                // RefreshTokenExpiry = atJwt.Claims.First(x => x.Type == "provider_refresh_token_expiry").Value,
                ConnectorId = atJwt.Claims.First(x => x.Type == "connector_id").Value
            };

            _repository.Add(state, consent);

            return consent;
        }

        public List<Consent> GetConsents(string userId)
        {
            return _repository.GetAll(userId);
        }
    }
}