using System;
using System.Threading.Tasks;
using HealthTracker.API.Models;

namespace HealthTracker.API.Services
{
    public interface IWaterService
    {
        Task<WaterLog> AddWaterLogAsync(int amountMl, DateTime date, string userId);
        Task<int> GetWaterConsumedAsync(DateTime date, string userId);
        Task UpdateDailyLogForWaterAsync(int amountMl, DateTime date, string userId);
    }
} 