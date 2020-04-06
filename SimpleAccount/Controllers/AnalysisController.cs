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
    public class AnalysisController : ControllerBase
    {
        private readonly IAnalysisService _analysisService;
        private readonly ILogger<AnalysisController> _logger;

        public AnalysisController(ILogger<AnalysisController> logger, IAnalysisService analysisService)
        {
            _logger = logger;
            _analysisService = analysisService;
        }

        [HttpGet("[action]", Name = "Analysis_Summary")]
        public async Task<CategorySummaryReport> SevenDaySummary(string userId)
        {
            var to = DateTime.UtcNow;
            var from = to.AddDays(-7);

            return await _analysisService.CategorySummary(userId, from, to);
        }
    }
}