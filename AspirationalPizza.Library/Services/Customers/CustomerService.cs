﻿using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AspirationalPizza.Library.Configuration;
using AspirationalPizza.Library.Services.Customers.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AspirationalPizza.Library.RepoSupport;

namespace AspirationalPizza.Library.Services.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<CustomerModel> _customerRepository;
        private readonly ILogger<CustomerService> _logger;
        private readonly IOptions<ServiceConfig<CustomerService>> _options;
        private readonly Mapper _mapper;

        public CustomerService(ILogger<CustomerService> logger, IRepository<CustomerModel> customerRepository, IOptions<ServiceConfig<CustomerService>> options)
        {
            _customerRepository = customerRepository;
            _logger = logger;
            _options = options;

            MapperConfiguration config = new MapperConfiguration(config =>
            {
                config.CreateMap<CustomerDto, CustomerModel>();
                config.CreateMap<CustomerModel, CustomerDto>();
                config.CreateMap<CustomerAddressDto, CustomerAddress>();
                config.CreateMap<CustomerAddress, CustomerAddressDto>();
            });
            _mapper = new Mapper(config);
        }

        public async Task<CustomerModel> CreateOrUpdate(CustomerModel customer)
        {
            if (String.IsNullOrEmpty(customer.CustomerId)) customer.CustomerId = Guid.NewGuid().ToString();
            if (await _customerRepository.Create(customer) != null) return customer;
            throw new Exception("New record was unable to be created");
        }

        public async Task<CustomerModel?> GetById(string id)
        {
            return await _customerRepository.Get(id);
        }

        public async Task<Boolean> Delete(CustomerModel customer)
        {
            return await _customerRepository.Delete(customer);
        }

        public async Task<List<CustomerModel>> Search(CustomerSearch searchObject)
        {
            return await _customerRepository.Search(searchObject);
        }

        public async Task<List<CustomerModel>> BulkInsert(List<CustomerModel> customerList)
        {
            return await _customerRepository.BulkInsert(customerList);
        }

        public void Dispose() { }

        public CustomerModel DtoToModel(CustomerDto dto) { return _mapper.Map<CustomerDto, CustomerModel>(dto); }
        public CustomerDto ModelToDto(CustomerModel model) { return _mapper.Map<CustomerModel, CustomerDto>(model); }

        
        //If the service needs two repositories we could add specific repository factories
        public static IRepository<CustomerModel> GetRepository(ILogger<IRepository<CustomerModel>> logger, ServiceConfig<CustomerService> config)
        {
            IRepository<CustomerModel>? returnValue = null;
            DbContextOptionsBuilder<CustomerDbContext> optionsBuilder = new DbContextOptionsBuilder<CustomerDbContext>();
            CustomerDbContext? context = null;
            String connectionString = String.Empty; 
            Configuration.RepoConfig? repoConfig = config!.Repositories["CustomerModel"];
            if (repoConfig == null) { throw new Exception("Could not find repository config, please configure the service."); }
            switch (repoConfig.RepositoryType)
            {
                case RepoTypes.Mongo:
                    //Use the mongo repo with its associated connection string information.
                    returnValue = new CustomerMongoRepository(logger, repoConfig);
                    return returnValue;
                case RepoTypes.Memory:
                    optionsBuilder.UseInMemoryDatabase("CustomerDB");
                    context = new CustomerDbContext(optionsBuilder.Options);
                    returnValue = new CustomerEFRepository(logger, context);
                    return returnValue;
                case RepoTypes.Sqlite:
                    //Add EF context here, and inject the EF context into the repo constructor to make EF happen.  
                    //  this actually provides a convenient wrapper around EF so that it could be switched to direct
                    //  sql implementations later if we wanted to skip the ORM.
                    connectionString = $"Data Source={repoConfig.Parameters["Filename"]}";
                    optionsBuilder.UseSqlite(connectionString);
                    context = new CustomerDbContext(optionsBuilder.Options);
                    returnValue = new CustomerEFRepository(logger, context);
                    return returnValue;
                case RepoTypes.Postgres:
                    connectionString = new StringBuilder()     // Yeah this is super lame but I liked the options tucked together
                        .Append($"Host={repoConfig.Parameters["DBHost"]};")
                        .Append($"Database={repoConfig.Parameters["DBName"]};")
                        .Append($"Username={repoConfig.Parameters["DBUser"]};")
                        .Append($"Password={repoConfig.Parameters["DBPass"]}")
                        .ToString();
                    context = new CustomerDbContext(optionsBuilder.Options);
                    returnValue = new CustomerEFRepository(logger, context);
                    return returnValue;
                default:
                    throw new NotImplementedException("The database specified in the config has not been implemented for this repository");
            }
        }
    }
}
