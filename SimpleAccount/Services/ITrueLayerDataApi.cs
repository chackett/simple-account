using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleAccount.DTO.Response;

namespace SimpleAccount.Services
{
    public interface ITrueLayerDataApi
    {
        /*
         * I believe the name of this interface to be controversial but would redesign a more generic interface
         * if other providers used.... but in a sense, it is arguable that TrueLayer is the more generic provider
         * of various banking services.
         */

        public string AuthorisationUrl(string state);
        
        Task<TrueLayerAccessToken> GetAccessToken(string oneTimeCode, string state);

        Task<List<Account>> GetAccounts(string accessToken);
        Task<Account> GetAccount(string accessToken, string accountId);
        Task<List<Transaction>> GetTransactions(string accessToken, string accountId, DateTime from, DateTime to);
    }
}