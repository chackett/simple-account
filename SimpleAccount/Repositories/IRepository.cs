using System.Collections.Generic;
using System.Dynamic;
using Microsoft.Extensions.Hosting.Internal;

namespace SimpleAccount.Repositories
{
    public interface IRepository<Object, Identifier>
    {
        List<Object> GetAll(Identifier id);
        
        Object Get(Identifier id);

        void Add(Identifier id, Object item);

        void Delete(Identifier id);

        void Update(Identifier id, Object item);
    }
}