using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AspirationalPizza.Library.RepoSupport;
using AspirationalPizza.Library.Configuration;

namespace AspirationalPizza.Library.Services.Customers.Repositories
{
    internal class CustomerMongoRepository : ICustomerRepository
    {
        private readonly ILogger<ICustomerRepository> _logger;
        private MongoClient _dbClient;
        IMongoDatabase _database;
        IMongoCollection<CustomerModel> _collection;

        public CustomerMongoRepository(ILogger<ICustomerRepository> logger, RepoConfig dbConfig)
        {
            _logger = logger;
            _dbClient = new MongoClient(dbConfig.Parameters["ConnectionString"]);
            _database = _dbClient.GetDatabase(dbConfig.Parameters["Database"]);
            _collection = _database.GetCollection<CustomerModel>(dbConfig.Parameters["Collection"]);

        }

        async Task<int> ICustomerRepository.Create(CustomerModel customer)
        {
            if (customer.CustomerId != null)
            {
                await _collection.InsertOneAsync(customer);
                return 1;
            }
            throw new ArgumentNullException(nameof(customer));
        }

        async Task<int> ICustomerRepository.Delete(CustomerModel customer)
        {
            FilterDefinition<CustomerModel> filter = Builders<CustomerModel>.Filter.Eq(c => c.CustomerId, customer.CustomerId);
            DeleteResult status = await _collection.DeleteOneAsync(filter);
            return (int)status.DeletedCount;
        }

        async Task<CustomerModel?> ICustomerRepository.Get(string customerId)
        {
            FilterDefinition<CustomerModel> filter = Builders<CustomerModel>.Filter.Eq(c => c.CustomerId, customerId);
            CustomerModel customer = await _collection.Find(filter).FirstOrDefaultAsync();
            return customer;
        }

        async Task<List<CustomerModel>> ICustomerRepository.Search(CustomerSearch searchObject)
        {
            FilterDefinition<CustomerModel> filter = MongoSearch<CustomerModel>.FilterBuilder(searchObject);
            List<CustomerModel> partyList = await _collection.Find(filter).ToListAsync();
            return partyList;
        }

        async Task<int> ICustomerRepository.Update(CustomerModel customer)
        {
            FilterDefinition<CustomerModel> filter = Builders<CustomerModel>.Filter.Eq(c => c.CustomerId, customer.CustomerId);
            ReplaceOneResult result = await _collection.ReplaceOneAsync(filter, customer);
            return (int)result.ModifiedCount;
        }
    }
}
