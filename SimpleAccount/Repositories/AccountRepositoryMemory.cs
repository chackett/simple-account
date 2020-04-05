using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.AspNetCore.Server.IIS.Core;
using SimpleAccount.DTO.Response;

namespace SimpleAccount.Repositories
{
    public class AccountRepositoryMemory : IRepository<Account, string>
    {
        private readonly Dictionary<string, Account> _db;

        public AccountRepositoryMemory()
        {
            _db = new Dictionary<string, Account>();
        }

        public List<Account> GetAll(string userId)
        {
            throw new System.NotImplementedException();
        }
        
        public Account Get(string consentId)
        {
            // if (!_db.ContainsKey(consentId))
            // {
            //     throw new Exception("consent not found");    
            // }
            //
            return _db[consentId];
        }

        public void Add(Account account)
        {
            // It is intentional that we can easily override previous consent for simplicity of this demo.
            // _db[consent.ConsentId] = account;
        }

        public void Delete(string consentId)
        {
            if (!_db.ContainsKey(consentId))
            {
                throw new Exception("consent not found");
            }

            _db.Remove(consentId);
        }

        public void Update(Account account)
        {
            // if (!_db.ContainsKey(consent.ConsentId))
            // {
            //     throw new Exception("consent not found");
            // }
            //
            // _db[consent.ConsentId] = consent;
        }
    }
}