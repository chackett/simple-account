using System.Dynamic;
using Microsoft.Extensions.Hosting.Internal;
using SimpleAccount.Domains;

namespace SimpleAccount.Repositories
{
    public interface IRepository<T>
    {
        Consent Get(string consentId);

        void Add(Consent consent);

        void Delete(string consentId);

        void Update(Consent consent);
    }
}