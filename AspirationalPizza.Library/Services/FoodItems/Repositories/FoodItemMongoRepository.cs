using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using AspirationalPizza.Library.RepoSupport;
using AspirationalPizza.Library.Configuration;
using Microsoft.EntityFrameworkCore;
using AspirationalPizza.Library.Services.Customers;

namespace AspirationalPizza.Library.Services.FoodItems.Repositories
{
    internal class FoodItemMongoRepository : IRepository<FoodItemModel>
    {
        private readonly ILogger<IRepository<FoodItemModel>> _logger;
        private MongoClient _dbClient;
        IMongoDatabase _database;
        IMongoCollection<FoodItemModel> _collection;

        public FoodItemMongoRepository(ILogger<IRepository<FoodItemModel>> logger, RepoConfig dbConfig)
        {
            _logger = logger;
            _dbClient = new MongoClient(dbConfig.Parameters["ConnectionString"]);
            _database = _dbClient.GetDatabase(dbConfig.Parameters["Database"]);
            _collection = _database.GetCollection<FoodItemModel>(dbConfig.Parameters["Collection"]);
        }

        async Task<FoodItemModel> IRepository<FoodItemModel>.Create(FoodItemModel foodItem)
        {
            try
            {
                await _collection.InsertOneAsync(foodItem);
                return foodItem;
            }
            catch (Exception ex) { throw new InvalidDataException("Unable to update specified food item record ", ex); }
        }

        async Task<Boolean> IRepository<FoodItemModel>.Delete(FoodItemModel foodItem)
        {
            FilterDefinition<FoodItemModel> filter = Builders<FoodItemModel>.Filter.Eq(f => f.FoodItemId, foodItem.FoodItemId);
            DeleteResult status = await _collection.DeleteOneAsync(filter);
            if (status.DeletedCount < 1) return true;
            throw new InvalidDataException("Unable to delete specified food item record ");
        }

        async Task<FoodItemModel?> IRepository<FoodItemModel>.Get(string foodItemId)
        {
            FilterDefinition<FoodItemModel> filter = Builders<FoodItemModel>.Filter.Eq(f => f.FoodItemId, foodItemId);
            FoodItemModel foodItem = await _collection.Find(filter).FirstOrDefaultAsync();
            return foodItem;
        }

        async Task<List<FoodItemModel>> IRepository<FoodItemModel>.Search(FoodItemSearch searchObject)
        {
            FilterDefinition<FoodItemModel> filter = MongoSearch<FoodItemModel>.FilterBuilder(searchObject);
            List<FoodItemModel> foodItemList = await _collection.Find(filter).ToListAsync();
            return foodItemList;
        }

        async Task<FoodItemModel> IRepository<FoodItemModel>.Update(FoodItemModel foodItem)
        {
            FilterDefinition<FoodItemModel> filter = Builders<FoodItemModel>.Filter.Eq(f => f.FoodItemId, foodItem.FoodItemId);
            ReplaceOneResult result = await _collection.ReplaceOneAsync(filter, foodItem);
            if (result.ModifiedCount < 1) return foodItem;
            throw new InvalidDataException("Unable to update specified food item record ");
        }

        async Task<List<FoodItemModel>> IRepository<FoodItemModel>.BulkInsert(List<FoodItemModel> foodItems)
        {
            try
            {
                foreach (FoodItemModel _foodItem in foodItems)
                    if (String.IsNullOrEmpty(_foodItem.FoodItemId)) _foodItem.FoodItemId = Guid.NewGuid().ToString();
                await _collection.InsertManyAsync(foodItems);
                return foodItems;
            }
            catch (Exception ex) { throw new InvalidDataException("Unable to create specified food item records ", ex); }
        }

        public void Dispose() { }  //Mongo Client handles dispose automatically.
    }
}
