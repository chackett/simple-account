using System.Collections.Generic;

namespace SimpleAccount.Repositories
{
    public interface IRepository<Object, Identifier>
    {
        List<Object> GetAll(Identifier id);

        Object Get(Identifier id);

        void Add(Identifier id, Object item);

        void Delete(Identifier id);

        void Update(Identifier id, Object item);
        
        // A marker to specify first use. A little hack, so that callers can check to see if they should bother 
        // querying this repo.
        bool Unused();
    }
}