using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using AspirationalPizza.Library.RepoSupport;
using AspirationalPizza.Library.Configuration;
using Microsoft.EntityFrameworkCore;

namespace AspirationalPizza.Library.Services.Customers.Repositories
{
    internal class CustomerMongoRepository : IRepository<CustomerModel>
    {
        private readonly ILogger<IRepository<CustomerModel>> _logger;
        private MongoClient _dbClient;
        IMongoDatabase _database;
        IMongoCollection<CustomerModel> _collection;

        public CustomerMongoRepository(ILogger<IRepository<CustomerModel>> logger, RepoConfig dbConfig)
        {
            _logger = logger;
            _dbClient = new MongoClient(dbConfig.Parameters["ConnectionString"]);
            _database = _dbClient.GetDatabase(dbConfig.Parameters["Database"]);
            _collection = _database.GetCollection<CustomerModel>(dbConfig.Parameters["Collection"]);

        }

        async Task<CustomerModel> IRepository<CustomerModel>.Create(CustomerModel customer)
        {
            try
            {
                await _collection.InsertOneAsync(customer);
                return customer;
            }
            catch (Exception ex) { throw new InvalidDataException("Unable to update specified customer record ", ex); }
        }

        async Task<Boolean> IRepository<CustomerModel>.Delete(CustomerModel customer)
        {
            FilterDefinition<CustomerModel> filter = Builders<CustomerModel>.Filter.Eq(c => c.CustomerId, customer.CustomerId);
            DeleteResult status = await _collection.DeleteOneAsync(filter);
            if (status.DeletedCount < 1) return true;
            throw new InvalidDataException("Unable to delete specified customer record ");
        }

        async Task<CustomerModel?> IRepository<CustomerModel>.Get(string customerId)
        {
            FilterDefinition<CustomerModel> filter = Builders<CustomerModel>.Filter.Eq(c => c.CustomerId, customerId);
            CustomerModel customer = await _collection.Find(filter).FirstOrDefaultAsync();
            return customer;
        }

        async Task<List<CustomerModel>> IRepository<CustomerModel>.Search(CustomerSearch searchObject)
        {
            FilterDefinition<CustomerModel> filter = MongoSearch<CustomerModel>.FilterBuilder(searchObject);
            List<CustomerModel> partyList = await _collection.Find(filter).ToListAsync();
            return partyList;
        }

        async Task<CustomerModel> IRepository<CustomerModel>.Update(CustomerModel customer)
        {
            FilterDefinition<CustomerModel> filter = Builders<CustomerModel>.Filter.Eq(c => c.CustomerId, customer.CustomerId);
            ReplaceOneResult result = await _collection.ReplaceOneAsync(filter, customer);
            if (result.ModifiedCount < 1) return customer;
            throw new InvalidDataException("Unable to update specified customer record ");
        }

        async Task<List<CustomerModel>> IRepository<CustomerModel>.BulkInsert(List<CustomerModel> customers)
        {
            try
            {
                foreach (CustomerModel _customer in customers) 
                    if (String.IsNullOrEmpty(_customer.CustomerId)) _customer.CustomerId = Guid.NewGuid().ToString();
                    await _collection.InsertManyAsync(customers);
                return customers;
            }
            catch (Exception ex) { throw new InvalidDataException("Unable to create specified customer records ", ex); }
        }

        public void Dispose() { }  //Mongo Client handles dispose automatically.
    }
}
