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
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

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

        async Task<CustomerModel> ICustomerRepository.Create(CustomerModel customer)
        {
            try
            {
                await _collection.InsertOneAsync(customer);
                return customer;
            }
            catch (Exception ex) { throw new InvalidDataException("Unable to update specified customer record ", ex); }
        }

        async Task<Boolean> ICustomerRepository.Delete(CustomerModel customer)
        {
            FilterDefinition<CustomerModel> filter = Builders<CustomerModel>.Filter.Eq(c => c.CustomerId, customer.CustomerId);
            DeleteResult status = await _collection.DeleteOneAsync(filter);
            if (status.DeletedCount < 1) return true;
            throw new InvalidDataException("Unable to delete specified customer record ");
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

        async Task<CustomerModel> ICustomerRepository.Update(CustomerModel customer)
        {
            FilterDefinition<CustomerModel> filter = Builders<CustomerModel>.Filter.Eq(c => c.CustomerId, customer.CustomerId);
            ReplaceOneResult result = await _collection.ReplaceOneAsync(filter, customer);
            if (result.ModifiedCount < 1) return customer;
            throw new InvalidDataException("Unable to update specified customer record ");
        }

        async Task<List<CustomerModel>> ICustomerRepository.BulkInsert(List<CustomerModel> customers)
        {
            try
            {
                await _collection.InsertManyAsync(customers);
                return customers;
            }
            catch (Exception ex) { throw new InvalidDataException("Unable to create specified customer records ", ex); }
        }

        public void Dispose() { }  //Mongo Client handles dispose automatically.
    }
}
