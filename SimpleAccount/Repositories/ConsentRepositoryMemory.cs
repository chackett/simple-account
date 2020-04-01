using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.AspNetCore.Server.IIS.Core;
using SimpleAccount.Domains;

namespace SimpleAccount.Repositories
{
    public class ConsentRepositoryMemory : IRepository<Consent>
    {
        private readonly Dictionary<string, Consent> _db;

        public ConsentRepositoryMemory()
        {
            _db = new Dictionary<string, Consent>();
        }

        public Consent Get(string consentId)
        {
            if (!_db.ContainsKey(consentId))
            {
                throw new Exception("consent not found");    
            }

            return _db[consentId];
        }

        public void Add(Consent consent)
        {
            // It is intentional that we can easily override previous consent for simplicity of this demo.
            _db[consent.ConsentId] = consent;
        }

        public void Delete(string consentId)
        {
            if (!_db.ContainsKey(consentId))
            {
                throw new Exception("consent not found");
            }

            _db.Remove(consentId);
        }

        public void Update(Consent consent)
        {
            if (!_db.ContainsKey(consent.ConsentId))
            {
                throw new Exception("consent not found");
            }

            _db[consent.ConsentId] = consent;
        }
    }
}