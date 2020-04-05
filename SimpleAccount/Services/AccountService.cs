using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleAccount.DTO.Response;
using SimpleAccount.Repositories;

namespace SimpleAccount.Services
{
    public class AccountService : IAccountService
    {
        private readonly ITrueLayerDataApi _trueLayerDataApi;
        private readonly IConsentService _consentService;
        private readonly IRepository<List<Account>, string> _accountRepository;
        private readonly IRepository<List<Transaction>, string> _transactionRepository;
        
        public AccountService(ITrueLayerDataApi trueLayerDataApi, IConsentService consentService,
            IRepository<List<Account>, string> accountRepository, IRepository<List<Transaction>, string> transactionRepository)
        {
            _trueLayerDataApi = trueLayerDataApi;
            _consentService = consentService;
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<List<Account>> GetAccounts(string userId, bool invalidateCache)
        {
            if (invalidateCache)
            {
                var accounts = await _trueLayerDataApi.GetAccounts(_consentService.GetConsent(userId).AccessTokenRaw);
                _accountRepository.Update(userId, accounts);
                return accounts;
            }
            try
            {
                return _accountRepository.Get(userId);
            }
            catch (Exception e)
            {
                var accounts = await _trueLayerDataApi.GetAccounts(_consentService.GetConsent(userId).AccessTokenRaw);
                _accountRepository.Update(userId, accounts);
                return accounts;
            }
        }

        public async Task<List<Transaction>> GetTransactions(string userId, string accountId, bool invalidateCache)
        {
            if (invalidateCache)
            {
                var transactions = await _trueLayerDataApi.GetTransactions(_consentService.GetConsent(userId).AccessTokenRaw, accountId);
                _transactionRepository.Update(accountId, transactions);
                return transactions;
            }
            try
            {
                return _transactionRepository.Get(accountId);
            }
            catch (Exception e)
            {
                var transactions = await _trueLayerDataApi.GetTransactions(_consentService.GetConsent(userId).AccessTokenRaw, accountId);
                _transactionRepository.Update(accountId, transactions);
                return transactions;
            }
        }
    }
}