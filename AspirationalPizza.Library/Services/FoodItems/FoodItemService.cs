using AspirationalPizza.Library.Configuration;
using AspirationalPizza.Library.RepoSupport;
using AspirationalPizza.Library.Services.FoodItems.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;

namespace AspirationalPizza.Library.Services.FoodItems
{
    //Copy #file:CustomerService.cs and replace Customer with FoodItem
    public class FoodItemService : IFoodItemService
    {
        private readonly IRepository<FoodItemModel> _foodItemRepository;
        private readonly ILogger<FoodItemService> _logger;
        private readonly IOptions<ServiceConfig<FoodItemService>> _options;
        private readonly Mapper _mapper;

        public FoodItemService(ILogger<FoodItemService> logger, IRepository<FoodItemModel> foodItemRepository, IOptions<ServiceConfig<FoodItemService>> options)
        {
            _foodItemRepository = foodItemRepository;
            _logger = logger;
            _options = options;

            MapperConfiguration config = new MapperConfiguration(config =>
            {
                config.CreateMap<FoodItemDto, FoodItemModel>();
                config.CreateMap<FoodItemModel, FoodItemDto>();
            });
            _mapper = new Mapper(config);
        }

        public async Task<FoodItemModel> CreateOrUpdate(FoodItemModel foodItem)
        {
            if (String.IsNullOrEmpty(foodItem.FoodItemId)) foodItem.FoodItemId = Guid.NewGuid().ToString();
            if (await _foodItemRepository.Create(foodItem) != null) return foodItem;
            throw new Exception("New record was unable to be created");
        }

        public async Task<FoodItemModel?> GetById(string FoodItemId)
        {
            return await _foodItemRepository.Get(FoodItemId);
        }

        public async Task<Boolean> Delete(FoodItemModel foodItem)
        {
            return await _foodItemRepository.Delete(foodItem);
        }

        public async Task<List<FoodItemModel>> Search(FoodItemSearch searchObject)
        {
            return await _foodItemRepository.Search(searchObject);
        }

        public FoodItemModel DtoToModel(FoodItemDto dto)
        {
            return _mapper.Map<FoodItemModel>(dto);
        }

        public FoodItemDto ModelToDto(FoodItemModel model)
        {
            return _mapper.Map<FoodItemDto>(model);
        }

        public async Task<List<FoodItemModel>> BulkInsert(List<FoodItemModel> foodItemList)
        {
            return await _foodItemRepository.BulkInsert(foodItemList);
        }

        public void Dispose() { }

        public static IRepository<FoodItemModel> GetRepository(ILogger<IRepository<FoodItemModel>> logger, ServiceConfig<FoodItemService> config)
        {
            IRepository<FoodItemModel>? returnValue = null;
            DbContextOptionsBuilder<FoodItemDbContext> optionsBuilder = new DbContextOptionsBuilder<FoodItemDbContext>();
            FoodItemDbContext? context = null;
            String connectionString = String.Empty;
            Configuration.RepoConfig? repoConfig = config!.Repositories["FoodServiceModel"];
            if (repoConfig == null) { throw new Exception("Could not find repository config, please configure the service."); }
            switch (repoConfig.RepositoryType)
            {
                case RepoTypes.Mongo:
                    returnValue = new FoodItemMongoRepository(logger, repoConfig);
                    return returnValue;
                case RepoTypes.Memory:
                    optionsBuilder.UseInMemoryDatabase("FoodItemDB");
                    context = new FoodItemDbContext(optionsBuilder.Options);
                    returnValue = new FoodItemEFRepository(logger, context);
                    return returnValue;
                case RepoTypes.Sqlite:
                    connectionString = $"Data Source={repoConfig.Parameters["Filename"]}";
                    optionsBuilder.UseSqlite(connectionString);
                    context = new FoodItemDbContext(optionsBuilder.Options);
                    returnValue = new FoodItemEFRepository(logger, context);
                    return returnValue;
                case RepoTypes.Postgres:
                    connectionString = new StringBuilder()
                        .Append($"Host={repoConfig.Parameters["DBHost"]};")
                        .Append($"Database={repoConfig.Parameters["DBName"]};")
                        .Append($"Username={repoConfig.Parameters["DBUser"]};")
                        .Append($"Password={repoConfig.Parameters["DBPass"]}")
                        .ToString();
                    context = new FoodItemDbContext(optionsBuilder.Options);
                    returnValue = new FoodItemEFRepository(logger, context);
                    return returnValue;
                default:
                    throw new NotImplementedException("The database specified in the config has not been implemented for this repository");
            }
        }
    }
}
