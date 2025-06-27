using System.Collections.Generic;
using System.Threading.Tasks;
using HealthTracker.API.Models;

namespace HealthTracker.API.Services
{
    public interface IFoodService
    {
        Task<IEnumerable<FoodItem>> SearchFoodsAsync(string query, string userId);
        Task<IEnumerable<FoodItem>> GetPersonalFoodsAsync(string userId);
        Task<FoodItem?> GetFoodByIdAsync(int id, string userId);
        Task<FoodItem> AddFoodAsync(FoodItem food, string userId);
    }
} 