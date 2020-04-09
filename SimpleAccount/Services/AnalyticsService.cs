using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleAccount.DTO.Response;

namespace SimpleAccount.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IAccountService _accountService;

        public AnalyticsService(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<CategorySummaryReport> CategorySummary(string userId, DateTime from, DateTime to, bool invalidateCache)
        {
            /* This function is a bit unwieldy, but I guess reporting usually is - needs a review */
            
            var accounts = await _accountService.GetAccounts(userId, invalidateCache);

            var transactions = new List<Transaction>();

            foreach (var account in accounts)
                transactions.AddRange(
                    await _accountService.GetTransactions(userId, invalidateCache, from, to));

            var values = new Dictionary<string, float>();

            foreach (var transaction in transactions)
            {
                float tmpVal;

                if (transaction.MerchantName == null)  continue;

                if(values.TryGetValue(transaction.MerchantName, out tmpVal))
                    values[transaction.MerchantName] = tmpVal + transaction.Amount;
                else
                    values[transaction.MerchantName] = transaction.Amount;
            }

            var result = new CategorySummaryReport();

            foreach (var value in values)
            {
                // If the debits and credits net off to zero, ignore the item.
                // It is a confusing UX to show that zero was spent at a merchant.
                if (value.Value == 0) continue;

                result.Result.Add(new ReportItem
                {
                    Category = value.Key,
                    Value = Math.Abs(value.Value) // Absolute value, because debits are shown as negative.
                });
            }

            return result;
        }
    }
}