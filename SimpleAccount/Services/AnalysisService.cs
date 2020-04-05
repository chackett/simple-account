using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleAccount.DTO.Response;

namespace SimpleAccount.Services
{
    public class AnalysisService : IAnalysisService
    {
        private readonly IAccountService _accountService;

        public AnalysisService(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<CategorySummaryReport> CategorySummary(string userId)
        {
            var accounts = await _accountService.GetAccounts(userId, false);

            var transactions = new List<SimpleAccount.DTO.Response.Transaction>();

            foreach (var account in accounts)
            { 
                transactions.AddRange(await _accountService.GetTransactions(userId, account.AccountId, false));
            }
            
            var values = new Dictionary<string, float>();

            foreach (var transaction in transactions)
            {
                float tmpVal;

                if (transaction.MerchantName != null)
                {
                    if (values.TryGetValue(transaction.MerchantName, out tmpVal))
                    {
                        values[transaction.MerchantName] = tmpVal+transaction.Amount;
                    }
                    else
                    {
                        values[transaction.MerchantName] = transaction.Amount;
                    }
                }
            }

            var result = new CategorySummaryReport();
            
            foreach(KeyValuePair<string, float> value in values)
            {
                if (value.Value == 0)
                {
                    // If the debits and credits net off to zero, ignore the item.
                    continue;
                }
                var ri = new ReportItem()
                {
                    Category = value.Key,
                    Value = Math.Abs(value.Value) //Absolute value, because debits are shown as negative.
                };
                result.Result.Add(ri);
            }
            
            return result;
        }
    }
}