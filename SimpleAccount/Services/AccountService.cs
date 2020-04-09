using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Extensions.Hosting.Internal;
using SimpleAccount.DTO.Response;
using SimpleAccount.Repositories;
using Transaction = SimpleAccount.DTO.Response.Transaction;

namespace SimpleAccount.Services
{
    public class AccountService : IAccountService
    {
        private readonly IRepository<List<Account>, string> _accountRepository;
        private readonly IConsentService _consentService;
        private readonly ITransactionRepository<Transaction, string> _transactionRepository;
        private readonly ITrueLayerDataApi _trueLayerDataApi;

        public AccountService(ITrueLayerDataApi trueLayerDataApi, IConsentService consentService,
            IRepository<List<Account>, string> accountRepository,
            ITransactionRepository<Transaction, string> transactionRepository)
        {
            _trueLayerDataApi = trueLayerDataApi;
            _consentService = consentService;
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<List<Account>> GetAccounts(string userId, bool invalidateCache)
        {
            return invalidateCache || _accountRepository.Unused()
                ? await RefreshAccounts(userId)
                : _accountRepository.Get(userId);
        }

        public async Task<List<Transaction>> GetTransactions(string userId, bool invalidateCache,
            DateTime from, DateTime to)
        {
            return invalidateCache || _transactionRepository.Unused()
                ? await RefreshTransactions(userId, from, to)
                : _transactionRepository.GetAll(userId);
        }
        
        private async Task<List<Account>> RefreshAccounts(string userId)
        {
            var result = new List<Account>();
            foreach (var consent in _consentService.GetConsents(userId))
            {
                var accounts = await _trueLayerDataApi.GetAccounts(consent.AccessTokenRaw);
                result.AddRange(accounts);
                _accountRepository.Update(userId, accounts);
                       
            }
            return result;
        }

        private async Task<List<Transaction>> RefreshTransactions(string userId, DateTime from, DateTime to)
        {
            // Will need an up to date account list, to refresh transactions.
            var accounts = await RefreshAccounts(userId);
            
            // We have a list of accounts, with a potential variety of account providers, meaning we need to
            // use the correct consent for the correct provider.
            // Convert the consents to a dictionary, with "connector id" as key. Which we can look up when iterating
            // the accounts.
            
            // Here's hoping "connector-id" is the same as "provider_id".

            var consents = _consentService.GetConsents(userId);
            var providerConsents = consents.ToDictionary(x => x.ConnectorId);
            
            // This approach will only support one provider each.
            var result = new List<Transaction>();
            foreach (var account in accounts)
            {
                var consent = providerConsents[account.Provider.ProviderId];
                var transactions = await _trueLayerDataApi.GetTransactions(consent.AccessTokenRaw, account.AccountId, from, to);
                result.AddRange(transactions);
                _transactionRepository.Add(userId, account.AccountId, result);
            }
            
            return result;
        }
    }
}