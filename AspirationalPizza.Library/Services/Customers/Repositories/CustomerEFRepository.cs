using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspirationalPizza.Library.RepoSupport;

namespace AspirationalPizza.Library.Services.Customers.Repositories
{
    internal class CustomerEFRepository : ICustomerRepository
    {
        private readonly CustomerDbContext _customerContext;
        private readonly ILogger<ICustomerRepository> _logger;

        public CustomerEFRepository(ILogger<ICustomerRepository> logger, CustomerDbContext customerContext)
        {
            _customerContext = customerContext;
            _logger = logger;
        }

        async Task<int> ICustomerRepository.Create(CustomerModel customer)
        {
            if (customer.CustomerId != null)
            {
                _customerContext.Customers.Add(customer);
                int updatedRows = await _customerContext.SaveChangesAsync();
                return updatedRows;
            }
            throw new ArgumentNullException(nameof(customer));
        }

        async Task<int> ICustomerRepository.Delete(CustomerModel customer)
        {
            _customerContext.Remove(customer);
            int updatedRows = await _customerContext.SaveChangesAsync();
            return updatedRows;
        }

        async Task<CustomerModel?> ICustomerRepository.Get(string customerId)
        {
            CustomerModel? got = await _customerContext.Customers.Where(c => c.CustomerId == customerId)
                .Include(p => p.CustomerAddresses)
                .FirstOrDefaultAsync();
            return got;
        }

        async Task<List<CustomerModel>> ICustomerRepository.Search(CustomerSearch searchObject)
        {
            //Not sure why the ToListAsync method doesn't show up here yet, but I'll live with the warning for now.
            return _customerContext.Customers.Where(LinqSearch<CustomerModel>.FilterBuilder(searchObject)).ToList();

            //Here are some notes for further reading:
            // LINK: https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/generate-consume-asynchronous-stream
        }

        async Task<int> ICustomerRepository.Update(CustomerModel customer)
        {
            if (customer.CustomerId != null)
            {
                _customerContext.Customers.Update(customer);
                int updatedRows = await _customerContext.SaveChangesAsync();
                return updatedRows;
            }
            throw new ArgumentNullException(nameof(customer));
        }
    }
}
