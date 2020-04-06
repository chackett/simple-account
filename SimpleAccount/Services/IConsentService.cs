using System.Threading.Tasks;

namespace SimpleAccount.Services
{
    public interface IConsentService
    {
        string AuthorisationUrl(string consentCorrelationId);
        Task CallbackAsync(string code, string state);
        Consent GetConsent(string userId);
    }
}