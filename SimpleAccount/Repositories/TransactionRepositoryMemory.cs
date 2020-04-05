using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.AspNetCore.Server.IIS.Core;
using SimpleAccount.DTO.Response;

namespace SimpleAccount.Repositories
{
    public class TransactionRepositoryMemory : IRepository<Transaction, string>
    {
        private readonly Dictionary<string, Transaction> _db;

        public TransactionRepositoryMemory()
        {
            _db = new Dictionary<string, Transaction>();
        }

        public List<Transaction> GetAll(string accountId)
        {
            throw new System.NotImplementedException();
        }
        
        public Transaction Get(string consentId)
        {
            if (!_db.ContainsKey(consentId))
            {
                throw new Exception("consent not found");    
            }

            return _db[consentId];
        }

        public void Add(Transaction transaction)
        {
            // It is intentional that we can easily override previous consent for simplicity of this demo.
            // _db[transaction.] = transaction;
        }

        public void Delete(string consentId)
        {
            // if (!_db.ContainsKey(consentId))
            // {
            //     throw new Exception("consent not found");
            // }
            //
            // _db.Remove(consentId);
        }

        public void Update(Transaction consent)
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