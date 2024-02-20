using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AspirationalPizza.Library.Services.FoodItems.Repositories
{
    internal class FoodItemEFRepository : IRepository<FoodItemModel>
    {
        private readonly FoodItemDbContext _foodItemContext;
        private readonly ILogger<IRepository<FoodItemModel>> _logger;

        public FoodItemEFRepository(ILogger<IRepository<FoodItemModel>> logger, FoodItemDbContext foodItemContext)
        {
            _foodItemContext = foodItemContext;
            _logger = logger;
        }

        async Task<FoodItemModel> IRepository<FoodItemModel>.Create(FoodItemModel foodItem)
        {
            if (foodItem.FoodItemId != null)
            {
                _foodItemContext.FoodItems.Add(foodItem);
                int updatedRows = await _foodItemContext.SaveChangesAsync();
                if (updatedRows > 0) return foodItem;
                throw new InvalidDataException("Unable to create new food item record ");
            }
            throw new ArgumentNullException(nameof(foodItem));
        }

        async Task<Boolean> IRepository<FoodItemModel>.Delete(FoodItemModel foodItem)
        {
            _foodItemContext.Remove(foodItem);
            int updatedRows = await _foodItemContext.SaveChangesAsync();
            if (updatedRows > 0) return true;
            throw new InvalidDataException("Unable to delete specified food item record ");
        }

        async Task<FoodItemModel?> IRepository<FoodItemModel>.Get(string foodItemId)
        {
            FoodItemModel? got = await _foodItemContext.FoodItems.FirstOrDefaultAsync(f => f.FoodItemId == foodItemId);
            return got;
        }

        async Task<List<FoodItemModel>> IRepository<FoodItemModel>.Search(FoodItemSearch searchObject)
        {
            return _foodItemContext.FoodItems.Where(RepoSupport.LinqSearch<FoodItemModel>.FilterBuilder(searchObject)).ToList();
        }

        async Task<FoodItemModel> IRepository<FoodItemModel>.Update(FoodItemModel foodItem)
        {
            if (foodItem.FoodItemId != null)
            {
                _foodItemContext.FoodItems.Update(foodItem);
                int updatedRows = await _foodItemContext.SaveChangesAsync();
                if (updatedRows > 0) return foodItem;
            }
            throw new ArgumentNullException(nameof(foodItem));
        }

        async Task<List<FoodItemModel>> IRepository<FoodItemModel>.BulkInsert(List<FoodItemModel> foodItems)
        {
            List<EntityEntry<FoodItemModel>> confirmed = new List<EntityEntry<FoodItemModel>>();
            List<FoodItemModel> returnList = new List<FoodItemModel>();
            foreach (FoodItemModel _foodItem in foodItems)
            {
                if (String.IsNullOrEmpty(_foodItem.FoodItemId)) _foodItem.FoodItemId = Guid.NewGuid().ToString();
                confirmed.Add(_foodItemContext.FoodItems.Add(_foodItem));
            }

            int updatedRows = await _foodItemContext.SaveChangesAsync();

            if (updatedRows > 0) 
                foreach (EntityEntry<FoodItemModel> _entry in confirmed) 
                    if (_entry.State.HasFlag(EntityState.Unchanged)) 
                        returnList.Add(_entry.Entity);

            if (foodItems.Count != returnList.Count) _logger
                    .LogWarning("Not all food item entries were added to the database from the bulk insert.");

            return returnList;
        }

        public void Dispose() { _foodItemContext.Dispose(); }
    }
}
