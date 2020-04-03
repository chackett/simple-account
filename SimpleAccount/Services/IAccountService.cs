using System.Collections.Generic;
using SimpleAccount.Domains;
using Transaction = System.Transactions.Transaction;

namespace SimpleAccount.Services
{
    public interface IAccountService
    {
        /*
         * This could be improved to add getters that return more detailed class definitions. And have the GetAlls()
         * just return summary objects, to improve efficiency. This is fine for task at hand.
         */
        
        List<Account> GetAccounts(Consent consent, bool invalidateCache);
        List<Transaction> GetTransactions(Consent consent, string accountId, bool invalidateCache);
    }
}