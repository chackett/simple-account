using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleAccount.DTO.Response;
using SimpleAccount.Services;

namespace SimpleAccount.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AnalysisController> _logger;
        private readonly IAccountService _accountService;

        public AccountController(ILogger<AnalysisController> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        [HttpGet("[action]", Name = "Account_Get_All")]
        public async Task<List<DTO.Response.Account>> Accounts(string userId = null, bool refresh = false)
        {
            // This use of userId as an argument, and being used for state is questionable and requires further
            // review but it serves it's purpose to avoid having to hard code users.
            if (string.IsNullOrEmpty(userId))
            {
                return null;
                // return new DTO.Response.Account
                // {
                // };
            }

            var accounts = await _accountService.GetAccounts(userId, refresh);

            return accounts;
        }

        [HttpGet("[action]", Name = "Account_Get+Transactions")]
        public async Task<List<DTO.Response.Transaction>> Transactions(string accountId, bool refresh = false, string userId = null)
        {
            // This use of userId as an argument, and being used for state is questionable and requires further
            // review but it serves it's purpose to avoid having to hard code users.
            if (string.IsNullOrEmpty(userId))
            {
                return null;
                // return new DTO.Response.Account
                // {
                // };
            }

            var transactions = await _accountService.GetTransactions(userId, accountId, refresh);

            return transactions;
        }
    }
}