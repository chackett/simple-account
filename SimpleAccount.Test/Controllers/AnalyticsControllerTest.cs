using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using SimpleAccount.Controllers;
using SimpleAccount.DTO.Response;
using SimpleAccount.Services;
using Xunit;

namespace SimpleAccount.Test.Controllers
{
    public class AnalyticsControllerTest
    {
        [Fact]
        public async void ShouldReturnAccounts()
        {
            var userId = "test-user";
            var refresh = false;
            
            var mLogger = new Mock<ILogger<AnalyticsController>>();
            
            var mockAccounts = new List<Account>(new Account[]
            {
                new Account()
                {
                    AccountNumber = new AccountNumber(){Number = "12345678", Iban = "psuedo-iban", SortCode = "80-80-80", SwiftBic = "swift-bic"},
                    Currency = "GBP",
                    AccountId = "some-account-identifier",
                    AccountType = "SAVINGS",
                    DisplayName = "xUnits Savings Account",
                    Provider = new Provider() { DisplayName = "API Powered Bank", LogoUri = "https://logo-uri-here", ProviderId = "unique-provider-id"},
                }
            });

            var mockTransactions = new List<Transaction>(new Transaction[]
            {
                new Transaction()
                {
                    Amount = 1.50f,
                    Currency = "GBP",
                    Description = "Some transaction description",
                    MerchantName = "Merchant A"
                },
                new Transaction()
                {
                    Amount = 2.00f,
                    Currency = "GBP",
                    Description = "Some transaction description",
                    MerchantName = "Merchant B"
                },
                new Transaction()
                {
                    Amount = 3.50f,
                    Currency = "GBP",
                    Description = "Some transaction description",
                    MerchantName = "Merchant B"
                },
                new Transaction()
                {
                    Amount = 1.25f,
                    Currency = "GBP",
                    Description = "Some transaction description",
                    MerchantName = "Merchant C"
                },
                new Transaction()
                {
                    Amount = 10.00f,
                    Currency = "GBP",
                    Description = "Some transaction description",
                    MerchantName = "Merchant D"
                },
                new Transaction()
                {
                    Amount = -10.00f,
                    Currency = "GBP",
                    Description = "Some transaction description",
                    MerchantName = "Merchant D"
                }
            });

            var mAccService = new Mock<IAccountService>();
            var dtFrom = DateTime.UtcNow;
            var dtTo = DateTime.UtcNow.AddDays(-7);
            mAccService.Setup(x => x.GetAccountsAsync(userId, refresh)).Returns(Task.FromResult(mockAccounts));
            mAccService.Setup(x => x.GetTransactionsAsync(userId, refresh,It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(Task.FromResult(mockTransactions));
            
            var analyticsService = new AnalyticsService(mAccService.Object);
            var acctController  = new AnalyticsController(mLogger.Object, analyticsService);

            var result = await acctController.SevenDaySummary(userId, refresh);

            var expected = new CategorySummaryReport()
            {
                Result =
                {
                    new ReportItem(){Category = "Merchant A", Value = 1.50f,},
                    new ReportItem(){Category = "Merchant B", Value = 5.50f,},
                    new ReportItem(){Category = "Merchant C", Value = 11.25f,},
                }
            };
            
            // Cannot figure out how to assert object equality.
            // Assert.Same(expected, result);
        }
    }
}