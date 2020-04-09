using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleAccount.Domains;

namespace SimpleAccount.Services
{
    public interface IConsentService
    {
        string AuthorisationUrl(string consentCorrelationId);
        Task<Consent> CallbackAsync(string code, string state);
        List<Consent> GetConsents(string userId);
    }
}