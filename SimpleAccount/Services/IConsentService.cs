using System.Threading.Tasks;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Hosting.Internal;
using SimpleAccount.Domains;

namespace SimpleAccount.Services
{
    public interface IConsentService
    {
        string AuthorisationUrl(string consentCorrelationId);
        Task CallbackAsync(string code, string state);
        Consent GetConsent(string userId);
    }
}