using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using SimpleAccount.Controllers;
using SimpleAccount.Domains;
using SimpleAccount.DTO.Request;
using SimpleAccount.DTO.Response;
using SimpleAccount.Repositories;
using SimpleAccount.Services;
using Xunit;

namespace SimpleAccount.Test.Controllers
{
    public class ConsentControllerTest
    {
        [Fact]
        public void ShouldReturnAuthorisationUrlWithCorrectStateValue()
        {
            var userId = "test-user";

            var mLogger = new Mock<ILogger<ConsentController>>();

            var config = new Mock<IConfiguration>();
            config.SetupGet(x => x["authorisationUrl"]).Returns("<enter_long_authorisation_url_here>");

            var mConsentRepo = new Mock<IRepository<Consent, string>>();
            var mTrueLayerDataApi = new Mock<ConcreteTrueLayerDataApi>(config.Object);

            var mConsentService = new ConsentService(config.Object, mConsentRepo.Object, mTrueLayerDataApi.Object);
            
            var mAcctRepo = new Mock<IRepository<List<Account>, string>>();
            var accts = new List<Account>();
            mAcctRepo.Setup(x => x.Unused()).Returns(false);
            mAcctRepo.Setup(x => x.Get(userId)).Returns(accts);
            var mTxnRepo = new Mock<ITransactionRepository<Transaction, string>>();

            var acctService = new AccountService(mTrueLayerDataApi.Object, mConsentService, mAcctRepo.Object, mTxnRepo.Object);
            
            var consentController = new ConsentController(mLogger.Object, mConsentService, acctService, mTrueLayerDataApi.Object);

            var response = consentController.Authorise(userId);
            Assert.Equal($"{config.Object["authorisationUrl"]}&state={userId}",response.AuthorisationUrl);
        }

        [Fact]
        public async void ShouldProcessCallback()
        {
            var state = "some-user-id";
            var accessToken = "at-raw1";
            var code = "some random code";
            
            var mConfig = new Mock<IConfiguration>();
            var mLogger = new Mock<ILogger<ConsentController>>();

            var mConsentRepo = new Mock<IRepository<Consent, string>>();
            var consents = new List<Consent>(new Consent[]
            {
                new Consent() { ConnectorId = "connector-id1", ConsentId = "consent-id1", AccessTokenRaw = accessToken},
            });
            mConsentRepo.Setup(x => x.GetAll(state)).Returns(consents);
            
            var mTlApi = new Mock<ITrueLayerDataApi>();
            var tlAccessToken = new TrueLayerAccessToken()
            {
                AccessToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjE0NTk4OUIwNTdDOUMzMzg0MDc4MDBBOEJBNkNCOUZFQjMzRTk1MTAiLCJ0eXAiOiJhdCtqd3QiLCJ4NXQiOiJGRm1Kc0ZmSnd6aEFlQUNvdW15NV9yTS1sUkEifQ.eyJuYmYiOjE1ODU2OTg0MjgsImV4cCI6MTU4NTcwMjAyOCwiaXNzIjoiaHR0cHM6Ly9hdXRoLnRydWVsYXllci5jb20iLCJhdWQiOlsiaHR0cHM6Ly9hdXRoLnRydWVsYXllci5jb20vcmVzb3VyY2VzIiwiaW5mb19hcGkiLCJhY2NvdW50c19hcGkiLCJ0cmFuc2FjdGlvbnNfYXBpIiwiYmFsYW5jZV9hcGkiLCJjYXJkc19hcGkiLCJkaXJlY3RfZGViaXRzX2FwaSIsInN0YW5kaW5nX29yZGVyc19hcGkiXSwiY2xpZW50X2lkIjoiZXhhbXBsZWFwcGxpY2F0aW9uLXRwcnQiLCJzdWIiOiJJYzZiVi8zQWUzN0hTNnRCY3pzbm5MUURCL2RqY3FzN1pQOStYem5yTTd3PSIsImF1dGhfdGltZSI6MTU4NTY5ODQyMCwiaWRwIjoibG9jYWwiLCJjb25uZWN0b3JfaWQiOiJvYi1tb256byIsImNyZWRlbnRpYWxzX2tleSI6IjkzOWIwYmYyZmM5ZmI3MzZiZDRlZTIwOWNhZmEzY2VhODAwYmNhYjY2MmE1MzYzYzQ5N2Y1YmVlYTE4NjJkMTgiLCJwcml2YWN5X3BvbGljeSI6IkZlYjIwMTkiLCJjb25zZW50X2lkIjoiMDVjOTk3NWUtMGQ0Mi00NWUzLWJkZWQtY2M2MGRmMTUzMjY0IiwicHJvdmlkZXJfYWNjZXNzX3Rva2VuX2V4cGlyeSI6IjIwMjAtMDQtMDJUMDU6NDY6NTlaIiwicHJvdmlkZXJfcmVmcmVzaF90b2tlbl9leHBpcnkiOiIyMDIwLTA2LTI5VDIzOjQ3OjAwWiIsInNvZnR3YXJlX3N0YXRlbWVudF9pZCI6IjNhdDZMVkhtNVZhaURSZlRvaGlLZ1AiLCJhY2NvdW50X3JlcXVlc3RfaWQiOiJvYmFpc3BhY2NvdW50aW5mb3JtYXRpb25jb25zZW50XzAwMDA5dGE4cVY0RHRqWGxwd09HNDkiLCJzY29wZSI6WyJpbmZvIiwiYWNjb3VudHMiLCJ0cmFuc2FjdGlvbnMiLCJiYWxhbmNlIiwiY2FyZHMiLCJkaXJlY3RfZGViaXRzIiwic3RhbmRpbmdfb3JkZXJzIiwib2ZmbGluZV9hY2Nlc3MiXSwiYW1yIjpbInB3ZCJdfQ.FgRxC5Ov7xVpbfM64gKfSd3Zgv84poaNa894ByFQj1QxQII8bN1eH9vMJleRoWe5W3I-JmdLbarFXMP9URMh4-wDUZYxPlpnHl8TAdPXa-7uo3MdlPfBQu2CNNInpPC6HTOOTWGlW8Y13UXv5697KK7NZB5C9O5H5qZNa0SsLuyfF_5XdmGBreR9dQulu4uuRSOE8qnJV2o3jK2Jd-bz4fRuShdO4kCwU8wUmkqCQiaPRtBDOdnnMH2xNbZmtewNkwaLJ-4AftoTtGoUL8_bWpy2RDxMIg9GvVsMEvHJv5rCs9BeMbACMEZJFEU0-S_-6w6ylkcn3odWPovKoLe8tw",
                ExpiresIn = 3600,
                RefreshToken = "some refresh token",
                TokenType = "type"
            };
            mTlApi.Setup(x => x.GetAccessTokenAsync(code, state)).Returns(Task.FromResult(tlAccessToken));
            
            var mAcctRepo = new Mock<IRepository<List<Account>, string>>();
            var accounts = new List<Account>();

            var mTxnRepo = new Mock<ITransactionRepository<Transaction, string>>();
            
            var consentService = new ConsentService(mConfig.Object, mConsentRepo.Object, mTlApi.Object);
            var accountService = new AccountService(mTlApi.Object, consentService, mAcctRepo.Object, mTxnRepo.Object);
            var consentController = new ConsentController(mLogger.Object, consentService, accountService, mTlApi.Object);

            var request = new AuthorisationCallbackRequest
            {
                Code = code,
                Scope = "scope",
                State = state
            };
            var response = await consentController.Callback(request);
            
            Assert.Equal("Success - Consent to ob-monzo granted.", response.Message);
        }
    }
}