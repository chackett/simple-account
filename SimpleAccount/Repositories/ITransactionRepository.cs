using System;
using System.Collections.Generic;

namespace SimpleAccount.Repositories
{
    // ITransactionRepository has been defined exclusively to handle access to transactions, as they are a little
    // less straightforward than the generic IRepository allows.
    // The high level idea is that the repository maintains a collection of transactions for various accounts,
    // under a single userId. So, one userId maps to a map of accountIds -> transaction list.
    public interface ITransactionRepository<Transaction, Identifier>
    {
        // Return a flattened list of transactions. i.e. All transactions for all accounts in one list.
        List<DTO.Response.Transaction> GetAll(Identifier userId);

        List<DTO.Response.Transaction> GetAccount(Identifier accountId);

        void Add(Identifier userId, Identifier accountId, List<DTO.Response.Transaction> transactions);

        void Delete(Identifier userId, Identifier accountId);

        void Update(Identifier accountId, List<DTO.Response.Transaction> item);

        // A marker to specify first use. A little hack, so that callers can check to see if they should bother 
        // querying this repo.
        bool Unused();
    }
}