using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthTracker.API.Data;
using HealthTracker.API.Models;

namespace HealthTracker.API.Services
{
    public class FoodService : IFoodService
    {
        private readonly AppDbContext _context;
        public FoodService(AppDbContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<FoodItem>> SearchFoodsAsync(string query, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));
            return Task.FromResult(_context.FoodItems.Where(f => f.Name.Contains(query) && (f.UserId == null || f.UserId == userId)).AsEnumerable());
        }

        public Task<IEnumerable<FoodItem>> GetPersonalFoodsAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));
            return Task.FromResult(_context.FoodItems.Where(f => f.UserId == userId).AsEnumerable());
        }

        public Task<FoodItem?> GetFoodByIdAsync(int id, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));
            return Task.FromResult(_context.FoodItems.FirstOrDefault(f => f.Id == id && (f.UserId == null || f.UserId == userId)));
        }

        public Task<FoodItem> AddFoodAsync(FoodItem food, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));
            food.UserId = userId;
            _context.FoodItems.Add(food);
            _context.SaveChanges();
            return Task.FromResult(food);
        }
    }
} 