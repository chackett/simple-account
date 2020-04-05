using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleAccount.DTO.Response;

namespace SimpleAccount.Services
{
    public interface IAccountService
    {
        /*
         * This could be improved to add getters that return more detailed class definitions. And have the GetAlls()
         * just return summary objects, to improve efficiency. This is fine for task at hand.
         */
        
        Task<List<Account>> GetAccounts(string userId, bool invalidateCache);
        List<Transaction> GetTransactions(string userId, string accountId, bool invalidateCache);
    }
}