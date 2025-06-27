using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthTracker.API.Models;

namespace HealthTracker.API.Services
{
    public interface IMealService
    {
        Task<Meal> AddMealAsync(Meal meal, string userId);
        Task<IEnumerable<Meal>> GetMealsByDateAsync(DateTime date, string userId);
        Task<Meal?> GetMealByIdAsync(int id, string userId);
        Task UpdateDailyLogForMealAsync(Meal meal, string userId);
    }
} 