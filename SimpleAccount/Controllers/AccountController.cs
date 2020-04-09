using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private readonly IAccountService _accountService;
        private readonly ILogger<AnalyticsController> _logger;

        public AccountController(ILogger<AnalyticsController> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        [HttpGet("[action]", Name = "Account_Get_All")]
        public async Task<List<Account>> Accounts(string userId = null, bool refresh = false)
        {
            // This use of userId as an argument, and being used for state is questionable and requires further
            // review but it serves it's purpose to avoid having to hard code users.
            return await _accountService.GetAccounts(userId, refresh);
        }

        [HttpGet("[action]", Name = "Account_Get+Transactions")]
        public async Task<List<Transaction>> Transactions(string userId, bool refresh = false)
        {
            // Can't figure out optional parameters that must be compile time constant
            var to = DateTime.UtcNow;
            var from = to.AddMonths(-3);

            return await _accountService.GetTransactions(userId, refresh, from, to);
        }
    }
}