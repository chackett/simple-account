using System;
using System.Collections.Generic;
using SimpleAccount.DTO.Response;

namespace SimpleAccount.Repositories
{
    public class TransactionRepositoryMemory : IRepository<List<Transaction>, string>
    {
        // Pushing the bounds here :/
        // [account_id] -> [transaction]
        private readonly Dictionary<string, List<Transaction>> _db;

        public TransactionRepositoryMemory()
        {
            _db = new Dictionary<string, List<Transaction>>();
        }

        public List<List<Transaction>> GetAll(string accountId)
        {
            var result = new List<List<Transaction>>();
            result.Add(_db[accountId]);

            return result;
        }
        
        public List<Transaction> Get(string accountId)
        {
            return _db[accountId];
        }

        public void Add(string accountId, List<Transaction> transactions)
        {
            _db[accountId] = transactions;
        }

        public void Delete(string accountId)
        {
            _db.Remove(accountId);
        }

        public void Update(string accountId, List<Transaction> transactions)
        {
            _db[accountId] = transactions;
        }
        
        
    }
}