using System.Dynamic;
using Microsoft.Extensions.Hosting.Internal;
using SimpleAccount.Domains;

namespace SimpleAccount.Repositories
{
    public interface IRepository<T>
    {
        T Get(string id);

        void Add(T item);

        void Delete(string id);

        void Update(T item);
    }
}