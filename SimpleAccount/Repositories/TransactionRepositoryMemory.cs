﻿using System.Collections.Generic;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.VisualBasic;
using SimpleAccount.DTO.Response;

namespace SimpleAccount.Repositories
{
    public class TransactionRepositoryMemory : ITransactionRepository<Transaction, string>
    {
        // userId -> accountId
        private readonly Dictionary<string, List<string>> _userAccounts;
        // accountId -> transactions
        private readonly Dictionary<string, List<Transaction>> _accountTransactions;

        public TransactionRepositoryMemory()
        {
            _userAccounts = new Dictionary<string, List<string>>();
            _accountTransactions = new Dictionary<string, List<Transaction>>();
        }

        public List<Transaction> GetAll(string userId)
        {
            var result = new List<DTO.Response.Transaction>();

            if (!_userAccounts.ContainsKey(userId))
            {
                return result;
            }
            
            var accounts = _userAccounts[userId];

            foreach (var account in accounts)
            {
                result.AddRange(_accountTransactions[account]);
            }

            return result;
        }

        public List<Transaction> GetAccount(string accountId)
        {
            return !_accountTransactions.ContainsKey(accountId) ? new List<Transaction>(0) : _accountTransactions[accountId];
        }

        public void Add(string userId, string accountId, List<Transaction> transactions)
        {
            if (!_userAccounts.ContainsKey(userId))
            {
                _userAccounts.Add(userId, new List<string>());
            }
            
            // Only add the user account if we are not aware of it yet.
            if (!_userAccounts[userId].Contains(accountId))
            {
                _userAccounts[userId].Add(accountId);    
            }
            _accountTransactions[accountId] = transactions;
        }

        public void Delete(string userId, string accountId)
        {
            if (_userAccounts.ContainsKey(userId) && _userAccounts[userId].Contains(accountId))
            {
                _userAccounts[userId].Remove(accountId);
            }
            _accountTransactions.Remove(accountId);
        }

        public void Update(string accountId, List<Transaction> transactions)
        {
            _accountTransactions[accountId] = transactions;
        }
    }
}