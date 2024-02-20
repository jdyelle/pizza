namespace AspirationalPizza.Library.Services.FoodItems.Repositories
{
    public interface IRepository<FoodItemModel> : IDisposable
    {
        Task<FoodItemModel?> Get(String foodItemId);
        Task<FoodItemModel> Create(FoodItemModel foodItem);
        Task<FoodItemModel> Update(FoodItemModel foodItem);
        Task<Boolean> Delete(FoodItemModel foodItem);
        Task<List<FoodItemModel>> BulkInsert(List<FoodItemModel> foodItemList);
        Task<List<FoodItemModel>> Search(FoodItemSearch searchObject);
        
    }
}