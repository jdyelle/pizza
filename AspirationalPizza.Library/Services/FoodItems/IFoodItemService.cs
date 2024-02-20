using AspirationalPizza.Library.Services.FoodItems.Repositories;

namespace AspirationalPizza.Library.Services.FoodItems
{
    public interface IFoodItemService
    {
        Task<FoodItemModel?> GetById(String foodItemId);
        Task<FoodItemModel> CreateOrUpdate(FoodItemModel foodItem);
        Task<List<FoodItemModel>> BulkInsert(List<FoodItemModel> customerList);
        Task<Boolean> Delete(FoodItemModel foodItem);
        Task<List<FoodItemModel>> Search(FoodItemSearch criteria);

        FoodItemModel DtoToModel(FoodItemDto dto);
        FoodItemDto ModelToDto(FoodItemModel model);
    }


}
