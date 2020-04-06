using System;
using System.Collections.Generic;
using SimpleAccount.Services;

namespace SimpleAccount.Repositories
{
    public class ConsentRepositoryMemory : IRepository<Consent, string>
    {
        private readonly Dictionary<string, Consent> _db;

        public ConsentRepositoryMemory()
        {
            _db = new Dictionary<string, Consent>();
        }

        public List<Consent> GetAll(string id)
        {
            throw new NotImplementedException();
        }

        public Consent Get(string userId)
        {
            if (!_db.ContainsKey(userId)) throw new Exception("user not found");

            return _db[userId];
        }

        public void Add(string userId, Consent consent)
        {
            _db[userId] = consent;
        }

        public void Delete(string userId)
        {
            if (!_db.ContainsKey(userId)) throw new Exception("user not found");

            _db.Remove(userId);
        }

        public void Update(string userId, Consent consent)
        {
            _db[userId] = consent;
        }
    }
}