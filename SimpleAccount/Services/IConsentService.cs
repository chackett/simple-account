using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleAccount.Domains;

namespace SimpleAccount.Services
{
    // This interface is loosely based on OAuth / Open ID flows. It is separated from Data access interfaces because:
    // * This interface implementation could be pointing towards a federated / single sign on type service.
    // * Typically, the smaller the interface the better.
    public interface IConsentService
    {
        string AuthorisationUrl(string consentCorrelationId);
        Task<Consent> CallbackAsync(string code, string state);
        List<Consent> GetConsents(string userId);
    }
}