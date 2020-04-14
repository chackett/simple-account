using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Moq;
using SimpleAccount.Domains;
using SimpleAccount.Repositories;
using SimpleAccount.Services;
using Xunit;

namespace SimpleAccount.Test.Services
{
    public class ConsentServiceTest
    {
        [Fact]
        public async void ShouldReturnListOfConsentsForUserId()
        {
            var userId = "some-user-id";
            var accessToken = "at-raw1";
            
            var mConfig = new Mock<IConfiguration>();

            var mConsentRepo = new Mock<IRepository<Consent, string>>();
            var consents = new List<Consent>(new Consent[]
            {
                new Consent() { ConnectorId = "connector-id1", ConsentId = "consent-id1", AccessTokenRaw = accessToken},
            });
            mConsentRepo.Setup(x => x.GetAll(userId)).Returns(consents);
            
            var mTlApi = new Mock<ITrueLayerDataApi>();

            var consentService = new ConsentService(mConfig.Object, mConsentRepo.Object, mTlApi.Object);

            var result = consentService.GetConsents(userId);
            
            Assert.Equal(consents, result);
        }
    }
}