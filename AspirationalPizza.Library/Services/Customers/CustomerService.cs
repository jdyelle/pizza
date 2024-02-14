using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AspirationalPizza.Library.Configuration;
using AspirationalPizza.Library.Services.Customers.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace AspirationalPizza.Library.Services.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<CustomerService> _logger;
        private readonly IOptions<ServiceConfig<CustomerService>> _options;
        private readonly Mapper _mapper;

        public CustomerService(ILogger<CustomerService> logger, ICustomerRepository customerRepository, IOptions<ServiceConfig<CustomerService>> options)
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

        public async Task<int> CreateOrUpdate(CustomerModel customer)
        {
            if (String.IsNullOrEmpty(customer.Id)) customer.Id = Guid.NewGuid().ToString();
            CustomerModel? _customer = await _customerRepository.Get(customer.Id);

            return await _customerRepository.Create(customer);
        }

        public async Task<CustomerModel?> GetById(string id)
        {
            return await _customerRepository.Get(id);
        }

        public async Task<int> Delete(CustomerModel customer)
        {
            return await _customerRepository.Delete(customer);
        }

        public async Task<List<CustomerModel>> Search(CustomerSearch searchObject)
        {
            return await _customerRepository.Search(searchObject);
        }

        public CustomerModel DtoToModel(CustomerDto dto) { return _mapper.Map<CustomerDto, CustomerModel>(dto); }
        public CustomerDto ModelToDto(CustomerModel model) { return _mapper.Map<CustomerModel, CustomerDto>(model); }

        //If the service needs two repositories we could add specific repository factories
        public static ICustomerRepository GetRepository(ILogger<ICustomerRepository> logger, ServiceConfig<CustomerService> config)
        {
            ICustomerRepository? returnValue = null;
            DbContextOptionsBuilder<CustomerDbContext> optionsBuilder = new DbContextOptionsBuilder<CustomerDbContext>();
            CustomerDbContext? context = null;
            String connectionString = String.Empty;
            if (config!.Repository!.RepositoryType == null) { throw new Exception("Could not find repository config, please configure the service."); }
            switch (config.Repository.RepositoryType)
            {
                case "Mongo":
                    //Use the mongo repo with its associated connection string information.
                    returnValue = new CustomerMongoRepository(logger, config.Repository);
                    return returnValue;
                case "Memory":
                    optionsBuilder.UseInMemoryDatabase("CustomerDB");
                    context = new CustomerDbContext(optionsBuilder.Options);
                    returnValue = new CustomerEFRepository(logger, context);
                    return returnValue;
                case "SQLite":
                    //Add EF context here, and inject the EF context into the repo constructor to make EF happen.  
                    //  this actually provides a convenient wrapper around EF so that it could be switched to direct
                    //  sql implementations later if we wanted to skip the ORM.
                    connectionString = $"Data Source={config.Repository.Parameters["Filename"]}";
                    optionsBuilder.UseSqlite(connectionString);
                    context = new CustomerDbContext(optionsBuilder.Options);
                    returnValue = new CustomerEFRepository(logger, context);
                    return returnValue;
                case "Postgres":
                    connectionString = new StringBuilder()     // Yeah this is super lame but I liked the options tucked together
                        .Append($"Host={config.Repository.Parameters["DBHost"]};")
                        .Append($"Database={config.Repository.Parameters["DBName"]};")
                        .Append($"Username={config.Repository.Parameters["DBUser"]};")
                        .Append($"Password={config.Repository.Parameters["DBPass"]}")
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
