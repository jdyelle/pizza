using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AspirationalPizza.Library.Services.Customers.Repositories;
using Amazon.Util.Internal.PlatformServices;
using Microsoft.Extensions.Options;
using AspirationalPizza.Library.Configuration;

namespace AspirationalPizza.Library.Services.Customers
{
    public class CustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<CustomerService> _logger;
        private readonly IOptions<ServiceConfig<CustomerService>> _options;

        public CustomerService(ILogger<CustomerService> logger, ICustomerRepository personRepository, IOptions<ServiceConfig<CustomerService>> options)
        {
            _customerRepository = personRepository;
            _logger = logger;
            _options = options;
        }

        public async Task<int> CreateOrUpdate(CustomerModel person)
        {
            if (String.IsNullOrEmpty(person.Id)) person.Id = Guid.NewGuid().ToString();
            CustomerModel? _person = await _customerRepository.Get(person.Id);

            return await _customerRepository.Create(person);

        }

        public async Task<CustomerModel?> GetById(string id)
        {
            return await _customerRepository.Get(id);
        }

        public async Task<int> Delete(CustomerModel person)
        {
            return await _customerRepository.Delete(person);
        }

        public async Task<List<CustomerModel>> Search(CustomerSearch searchObject)
        {
            return await _customerRepository.Search(searchObject);
        }

    }
}
