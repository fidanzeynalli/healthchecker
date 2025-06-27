using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthTracker.API.Data;
using HealthTracker.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthTracker.API.Services
{
    public class MealService : IMealService
    {
        private readonly AppDbContext _context;
        public MealService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Meal> AddMealAsync(Meal meal, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));
            meal.UserId = userId;
            _context.Meals.Add(meal);
            await _context.SaveChangesAsync();
            await UpdateDailyLogForMealAsync(meal, userId);
            return meal;
        }

        public Task<IEnumerable<Meal>> GetMealsByDateAsync(DateTime date, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));
            return Task.FromResult(_context.Meals.Include(m => m.Items).ThenInclude(i => i.FoodItem)
                .Where(m => m.UserId == userId && m.CreatedDate.Date == date.Date).AsEnumerable());
        }

        public Task<Meal?> GetMealByIdAsync(int id, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));
            return Task.FromResult(_context.Meals.Include(m => m.Items).ThenInclude(i => i.FoodItem)
                .FirstOrDefault(m => m.Id == id && m.UserId == userId));
        }

        public async Task UpdateDailyLogForMealAsync(Meal meal, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));
            var date = meal.CreatedDate.Date;
            var log = _context.DailyLogs.FirstOrDefault(l => l.UserId == userId && l.Date == date);
            if (log == null)
            {
                log = new DailyLog { UserId = userId, Date = date };
                _context.DailyLogs.Add(log);
            }
            // Sum all meals for the day
            var meals = _context.Meals.Include(m => m.Items).ThenInclude(i => i.FoodItem)
                .Where(m => m.UserId == userId && m.CreatedDate.Date == date).ToList();
            log.TotalCalories = meals.SelectMany(m => m.Items).Sum(i => i.FoodItem.CaloriesPerServing * (int)i.Quantity);
            log.TotalCarb = meals.SelectMany(m => m.Items).Sum(i => i.FoodItem.Carb * (int)i.Quantity);
            log.TotalFat = meals.SelectMany(m => m.Items).Sum(i => i.FoodItem.Fat * (int)i.Quantity);
            log.TotalProtein = meals.SelectMany(m => m.Items).Sum(i => i.FoodItem.Protein * (int)i.Quantity);
            await _context.SaveChangesAsync();
        }
    }
} 