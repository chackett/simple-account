using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleAccount.DTO.Response;
using SimpleAccount.Services;

namespace SimpleAccount.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;
        private readonly ILogger<AnalyticsController> _logger;

        public AnalyticsController(ILogger<AnalyticsController> logger, IAnalyticsService analyticsService)
        {
            _logger = logger;
            _analyticsService = analyticsService;
        }

        [HttpGet("[action]", Name = "Analytics_Summary")]
        public async Task<CategorySummaryReport> SevenDaySummary(string userId, bool refresh = false)
        {
            var to = DateTime.UtcNow;
            var from = to.AddDays(-7);

            return await _analyticsService.CategorySummary(userId, from, to, refresh);
        }
    }
}