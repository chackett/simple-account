using System.Collections.Generic;
using SimpleAccount.Domains;
using SimpleAccount.Repositories;

namespace SimpleAccount.Services
{
    public class SimpleAccountCache<Object, Identifier> : ICache<Object, Identifier>
    {
        private readonly IRepository<Object, Identifier> _repository;
        private readonly Dictionary<Identifier, Object> _cache;
        private readonly int _ttlMs;

        public SimpleAccountCache(IRepository<Object, Identifier> repository)
        {
            _repository = repository;
            _cache = new Dictionary<Identifier, Object>();
            // _ttlMs = ttlMs;
        }

        public Object Retrieve(bool invalidate, Identifier identifier)
        {
            return invalidate ? _repository.Get(identifier) : _cache[identifier];
        }
    }
}