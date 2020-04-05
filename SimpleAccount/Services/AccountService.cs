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
        private readonly IRepository<Account, string> _accountRepository;
        private readonly IRepository<Transaction, string> _transactionRepository;
        
        public AccountService(ITrueLayerDataApi trueLayerDataApi, IConsentService consentService,
            IRepository<Account, string> accountRepository, IRepository<Transaction, string> transactionRepository)
        {
            _trueLayerDataApi = trueLayerDataApi;
            _consentService = consentService;
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<List<Account>> GetAccounts(string userId, bool invalidateCache)
        {
            // if (invalidateCache)
            // {
                return await _trueLayerDataApi.GetAccounts(_consentService.GetConsent(userId).AccessTokenRaw);
            // }

            // return _accountRepository.GetAll(userId);
        }

        public List<Transaction> GetTransactions(string userId, string accountId, bool invalidateCache)
        {
            throw new System.NotImplementedException();
        }
    }
}