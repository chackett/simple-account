using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using SimpleAccount.Controllers;
using SimpleAccount.DTO.Response;
using SimpleAccount.Repositories;
using SimpleAccount.Services;
using Xunit;

namespace SimpleAccount.Test.Controllers
{
    public class AccountControllerTest
    {
        [Fact]
        public async void ShouldReturnAccounts()
        {
            var userId = "test-user";
            var refresh = false;
            
            var mLogger = new Mock<ILogger<AccountController>>();

            var config = new Mock<IConfiguration>();

            var mTlApi = new Mock<ConcreteTrueLayerDataApi>(config.Object);
            var mConsentSvc = new Mock<IConsentService>();
            var mAcctRepo = new Mock<IRepository<List<Account>, string>>();
            var accts = new List<Account>();
            mAcctRepo.Setup(x => x.Unused()).Returns(false);
            mAcctRepo.Setup(x => x.Get(userId)).Returns(accts);
            
            var mTxnRepo = new Mock<ITransactionRepository<Transaction, string>>();
            
            var acctService = new AccountService(mTlApi.Object, mConsentSvc.Object, mAcctRepo.Object, mTxnRepo.Object);
            var acctController  = new AccountController(mLogger.Object, acctService);

            var accounts = await acctController.Accounts(userId, refresh);
            Assert.Equal(accts, accounts);
        }

        [Fact]
        public async void ShouldReturnTransactions()
        {
            var userId = "test-user";
            var accountId = "123";
            var refresh = false;
            
            var mLogger = new Mock<ILogger<AccountController>>();

            var config = new Mock<IConfiguration>();
            
            var mTlApi = new Mock<ConcreteTrueLayerDataApi>(config.Object);
            var mConsentSvc = new Mock<IConsentService>();
            var mAcctRepo = new Mock<IRepository<List<Account>, string>>();
            var accounts = new List<Account>();
            mAcctRepo.Setup(x => x.Unused()).Returns(false);
            mAcctRepo.Setup(x => x.Get(userId)).Returns(accounts);
            
            var mTxnRepo = new Mock<ITransactionRepository<Transaction, string>>();
            var transactions = new List<Transaction>();
            transactions.Add(new Transaction()
            {
                Category = "shopping",
                Amount = 1.23f,
                Currency = "GBP",
                MerchantName = "Test Merchant"
            });
            
            mTxnRepo.Setup(x => x.Unused()).Returns(false);
            mTxnRepo.Setup(x => x.GetAll(userId)).Returns(transactions);
            
            var acctService = new AccountService(mTlApi.Object, mConsentSvc.Object, mAcctRepo.Object, mTxnRepo.Object);
            var acctController  = new AccountController(mLogger.Object, acctService);

            var result = await acctController.Transactions(userId, refresh);
            Assert.Equal(transactions, result);
        }
    }
}
