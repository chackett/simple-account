using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using SimpleAccount.Domains;
using SimpleAccount.DTO.Response;
using SimpleAccount.Repositories;
using SimpleAccount.Services;
using Xunit;

namespace SimpleAccount.Test.Services
{
    public class AccountServiceTest
    {
        [Fact]
        public async void GetAccountsShouldReturnFromRepositoryAndNotCallTrueLayerApi()
        {
            // Setup
             var userId = "test-user";
            var refresh = true;
            var accessToken = "some-access-token";
            
            var mConsentSvc = new Mock<IConsentService>();
            var consents = new List<Consent>(new Consent[] { new Consent(){AccessTokenRaw =  accessToken, ConnectorId = "unique-provider-id"} });
            mConsentSvc.Setup(x => x.GetConsents(userId)).Returns(consents);
            var mTxnRepo = new Mock<ITransactionRepository<Transaction, string>>();
            
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
            
            var mAcctRepo = new Mock<IRepository<List<Account>, string>>();
            mAcctRepo.Setup(x => x.Unused()).Returns(false);
            mAcctRepo.Setup(x => x.Get(userId)).Returns(mockAccounts);
            
            var mTlApi = new Mock<ITrueLayerDataApi>();

            var acctService = new AccountService(mTlApi.Object, mConsentSvc.Object, mAcctRepo.Object, mTxnRepo.Object);
            
            // Function call
            var result = await acctService.GetAccountsAsync(userId, false);
            
            // Assert
            Assert.Equal(mockAccounts, result);
            mTlApi.Verify(x => x.GetAccountsAsync(accessToken), Times.Never);
            mAcctRepo.Verify(x => x.Get(userId), Times.Once);
        }

        [Fact]
        public async void GetAccountsShouldReturnAccountsFromTrueLayerApiAndNotReturnFromRepository()
        {
            // Setup
            var userId = "test-user";
            var refresh = true;
            var accessToken = "some-access-token";
            
            var mConsentSvc = new Mock<IConsentService>();
            var consents = new List<Consent>(new Consent[] { new Consent(){AccessTokenRaw =  accessToken, ConnectorId = "unique-provider-id"} });
            mConsentSvc.Setup(x => x.GetConsents(userId)).Returns(consents);
            var mTxnRepo = new Mock<ITransactionRepository<Transaction, string>>();
            
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
            
            var mAcctRepo = new Mock<IRepository<List<Account>, string>>();
            mAcctRepo.Setup(x => x.Unused()).Returns(true);
            mAcctRepo.Setup(x => x.Get(userId)).Returns(mockAccounts);
            
            var mTlApi = new Mock<ITrueLayerDataApi>();
            mTlApi.Setup(x => x.GetAccountsAsync(accessToken)).Returns(Task.FromResult(mockAccounts));

            var acctService = new AccountService(mTlApi.Object, mConsentSvc.Object, mAcctRepo.Object, mTxnRepo.Object);
            
            // Function call
            var result = await acctService.GetAccountsAsync(userId, false);
            
            // Assert
            Assert.Equal(mockAccounts, result);
            mTlApi.Verify(x => x.GetAccountsAsync(accessToken), Times.Once);
            mAcctRepo.Verify(x => x.Get(userId), Times.Never);
        }

        [Fact]
        public async void GetTransactionsShouldReturnFromRepositoryAndNotCallTrueLayerApi()
        {
            // Setup
            var userId = "test-user";
            var refresh = true;
            var accessToken = "some-access-token";
            
            var mConsentSvc = new Mock<IConsentService>();
            var consents = new List<Consent>(new Consent[] { new Consent(){AccessTokenRaw =  accessToken, ConnectorId = "unique-provider-id"} });
            mConsentSvc.Setup(x => x.GetConsents(userId)).Returns(consents);
            
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
                    Amount = 12.34f,
                    Currency = "GBP",
                    Description = "Some transaction description",
                    MerchantName = "Some merchant name"
                }
            });

            var mTxnRepo = new Mock<ITransactionRepository<Transaction, string>>();
            mTxnRepo.Setup(x => x.Unused()).Returns(false);
            mTxnRepo.Setup(x => x.GetAll(userId)).Returns(mockTransactions);
            
            var mAcctRepo = new Mock<IRepository<List<Account>, string>>();
            mAcctRepo.Setup(x => x.Unused()).Returns(false);
            mAcctRepo.Setup(x => x.Get(userId)).Returns(mockAccounts);
            
            var dtFrom = DateTime.UtcNow;
            var dtTo = DateTime.UtcNow.AddMonths(-3);
            
            var mTlApi = new Mock<ITrueLayerDataApi>();
            mTlApi.Setup(x => x.GetAccountsAsync(accessToken)).Returns(Task.FromResult(mockAccounts));
            mTlApi.Setup(x => x.GetTransactionsAsync(accessToken, mockAccounts[0].AccountId, dtFrom, dtTo)).Returns(Task.FromResult(mockTransactions));
            
            var acctService = new AccountService(mTlApi.Object, mConsentSvc.Object, mAcctRepo.Object, mTxnRepo.Object);
            
            // Function call
            var result = await acctService.GetTransactionsAsync(userId, false, dtFrom, dtTo);
            
            // Assert
            Assert.Equal(mockTransactions, result);
            mTxnRepo.Verify(x => x.GetAll(userId), Times.Once);
            mTlApi.Verify(x => x.GetAccountsAsync(accessToken), Times.Never);
        }

        [Fact]
        public async void GetTransactionsShouldReturnFromTrueLayerApiAndNotReturnFromRepository()
        {
            // Setup
            var userId = "test-user";
            var refresh = true;
            var accessToken = "some-access-token";
            
            var mConsentSvc = new Mock<IConsentService>();
            var consents = new List<Consent>(new Consent[] { new Consent(){AccessTokenRaw =  accessToken, ConnectorId = "unique-provider-id"} });
            mConsentSvc.Setup(x => x.GetConsents(userId)).Returns(consents);
            
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
                    Amount = 12.34f,
                    Currency = "GBP",
                    Description = "Some transaction description",
                    MerchantName = "Some merchant name"
                }
            });

            var mTxnRepo = new Mock<ITransactionRepository<Transaction, string>>();
            mTxnRepo.Setup(x => x.Unused()).Returns(true);
            mTxnRepo.Setup(x => x.GetAll(userId)).Returns(mockTransactions);
            
            var mAcctRepo = new Mock<IRepository<List<Account>, string>>();
            mAcctRepo.Setup(x => x.Unused()).Returns(true);
            mAcctRepo.Setup(x => x.Get(userId)).Returns(mockAccounts);
            
            var dtFrom = DateTime.UtcNow;
            var dtTo = DateTime.UtcNow.AddMonths(-3);
            
            var mTlApi = new Mock<ITrueLayerDataApi>();
            mTlApi.Setup(x => x.GetAccountsAsync(accessToken)).Returns(Task.FromResult(mockAccounts));
            mTlApi.Setup(x => x.GetTransactionsAsync(accessToken, mockAccounts[0].AccountId, dtFrom, dtTo)).Returns(Task.FromResult(mockTransactions));
            
            var acctService = new AccountService(mTlApi.Object, mConsentSvc.Object, mAcctRepo.Object, mTxnRepo.Object);
            
            // Function call
            var result = await acctService.GetTransactionsAsync(userId, false, dtFrom, dtTo);
            
            // Assert
            Assert.Equal(mockTransactions, result);
            mTxnRepo.Verify(x => x.GetAll(userId), Times.Never);
            mTlApi.Verify(x => x.GetAccountsAsync(accessToken), Times.Once);
        }
    }
}