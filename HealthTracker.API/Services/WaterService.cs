using System;
using System.Linq;
using System.Threading.Tasks;
using HealthTracker.API.Data;
using HealthTracker.API.Models;

namespace HealthTracker.API.Services
{
    public class WaterService : IWaterService
    {
        private readonly AppDbContext _context;
        public WaterService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<WaterLog> AddWaterLogAsync(int amount, DateTime date, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));
            var log = new WaterLog { UserId = userId, Date = date, AmountMl = amount };
            _context.WaterLogs.Add(log);
            await _context.SaveChangesAsync();
            await UpdateDailyLogForWaterAsync(amount, date, userId);
            return log;
        }

        public Task<int> GetWaterConsumedAsync(DateTime date, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));
            return Task.FromResult(_context.WaterLogs.Where(w => w.UserId == userId && w.Date.Date == date.Date).Sum(w => w.AmountMl));
        }

        public async Task UpdateDailyLogForWaterAsync(int amount, DateTime date, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));
            var log = _context.DailyLogs.FirstOrDefault(l => l.UserId == userId && l.Date == date.Date);
            if (log == null)
            {
                log = new DailyLog { UserId = userId, Date = date, TotalWaterMl = 0 };
                _context.DailyLogs.Add(log);
            }
            log.TotalWaterMl = _context.WaterLogs.Where(w => w.UserId == userId && w.Date.Date == date.Date).Sum(w => w.AmountMl) + amount;
            await _context.SaveChangesAsync();
        }
    }
} 