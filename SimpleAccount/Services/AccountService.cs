using System.Collections.Generic;
using SimpleAccount.Domains;
using Transaction = System.Transactions.Transaction;

namespace SimpleAccount.Services
{
    public class AccountService : IAccountService
    {
        private readonly ITrueLayerDataApi _trueLayerDataApi;
        private readonly ICache<Account, string> _accountCache;
        private readonly ICache<Transaction, string> _transactionCache;
        
        //public AccountService(ITrueLayerDataApi trueLayerDataApi, ICache<Account, string> accountCache, ICache<Transaction, string> transactionCache)
        public AccountService(ITrueLayerDataApi trueLayerDataApi)
        {
            _trueLayerDataApi = trueLayerDataApi;
            // _accountCache = accountCache;
            // _transactionCache = transactionCache;
        }

        public List<Account> GetAccounts(Consent consent, bool invalidateCache)
        {
            throw new System.NotImplementedException();
        }

        public List<Transaction> GetTransactions(Consent consent, string accountId, bool invalidateCache)
        {
            throw new System.NotImplementedException();
        }
    }
}