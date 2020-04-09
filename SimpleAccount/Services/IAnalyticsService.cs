using System;
using System.Threading.Tasks;
using SimpleAccount.DTO.Response;

namespace SimpleAccount.Services
{
    public interface IAnalyticsService
    {
        Task<CategorySummaryReport> CategorySummary(string userId, DateTime from, DateTime to, bool invalidateCache);
    }
}