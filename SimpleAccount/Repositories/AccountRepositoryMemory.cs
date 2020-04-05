using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.AspNetCore.Server.IIS.Core;
using SimpleAccount.DTO.Response;

namespace SimpleAccount.Repositories
{
    public class AccountRepositoryMemory : IRepository<List<Account>, string>
    {
        private readonly Dictionary<string, List<Account>> _db;

        public AccountRepositoryMemory()
        {
            _db = new Dictionary<string, List<Account>>();
        }

        public List<List<Account>> GetAll(string userId)
        {
            var result = new List<List<Account>>();
            result.Add(_db[userId]);

            return result;
        }

        public List<Account> Get(string userId)
        {
            return _db[userId];
        }

        public void Add(string accountId, List<Account> account)
        {
            _db[accountId] = account;
        }

        public void Delete(string consentId)
        {
            _db.Remove(consentId);
        }

        public void Update(string accountId, List<Account> account)
        {
            _db[accountId] = account;
        }
    }
}