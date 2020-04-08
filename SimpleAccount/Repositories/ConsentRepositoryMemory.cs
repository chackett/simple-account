using System;
using System.Collections.Generic;
using System.Linq;
using SimpleAccount.Services;

namespace SimpleAccount.Repositories
{
    public class ConsentRepositoryMemory : IRepository<Consent, string>
    {
        // [userId]-> [map[connector-id]-> Consent]
        // A map of userId mapping to map of connector ids mapping to a Consent object
        // i.e. Support multiple consents/providers for a single user
        private readonly Dictionary<string, Dictionary<string, Consent>> _db;

        public ConsentRepositoryMemory()
        {
            _db = new Dictionary<string, Dictionary<string, Consent>>();
        }

        public Consent Get(string userId)
        {
            throw new NotImplementedException();
        }

        public List<Consent> GetAll(string userId)
        {
            if (!_db.ContainsKey(userId)) throw new Exception("user not found");

            return _db[userId].Values.ToList();
        }

        public void Add(string userId, Consent consent)
        {
            if (!_db.ContainsKey(userId)) _db[userId] = new Dictionary<string, Consent>();

            _db[userId][consent.ConnectorId] = consent;
        }

        public void Delete(string userId)
        {
            if (!_db.ContainsKey(userId)) throw new Exception("user not found");

            _db.Remove(userId);
        }

        public void Update(string userId, Consent consent)
        {
            if (!_db.ContainsKey(userId)) throw new Exception("user not found");

            _db[userId][consent.ConnectorId] = consent;
        }
    }
}