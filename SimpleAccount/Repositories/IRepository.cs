using System.Dynamic;
using Microsoft.Extensions.Hosting.Internal;
using SimpleAccount.Domains;

namespace SimpleAccount.Repositories
{
    public interface IRepository<Object, Identifier>
    {
        Object Get(Identifier id);

        void Add(Object item);

        void Delete(string id);

        void Update(Object item);
    }
}