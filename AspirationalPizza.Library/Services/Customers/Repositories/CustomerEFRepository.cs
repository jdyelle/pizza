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

        public CustomerEFRepository(ILogger<ICustomerRepository> logger, CustomerDbContext personContext)
        {
            _customerContext = personContext;
            _logger = logger;
        }

        async Task<int> ICustomerRepository.Create(CustomerModel person)
        {
            if (person.Id != null)
            {
                _customerContext.Customers.Add(person);
                int updatedRows = await _customerContext.SaveChangesAsync();
                return updatedRows;
            }
            throw new ArgumentNullException(nameof(person));
        }

        async Task<int> ICustomerRepository.Delete(CustomerModel person)
        {
            _customerContext.Remove(person);
            int updatedRows = await _customerContext.SaveChangesAsync();
            return updatedRows;
        }

        async Task<CustomerModel?> ICustomerRepository.Get(string personId)
        {
            CustomerModel? got = await _customerContext.Customers.Where(p => p.Id == personId)
                .Include(p => p.CustomerAddresses)
                .Include(p => p.Emails)
                .Include(p => p.PhoneNumbers)
                .Include(p => p.FavoriteFoodItems)
                .FirstOrDefaultAsync();
            if (got == null) throw new ArgumentNullException(nameof(got));
            return got;
        }

        async Task<List<CustomerModel>> ICustomerRepository.Search(CustomerSearch searchObject)
        {
            //Not sure why the ToListAsync method doesn't show up here yet, but I'll live with the warning for now.
            return _customerContext.Customers.Where(LinqSearch<CustomerModel>.FilterBuilder(searchObject)).ToList();

            //Here are some notes for further reading:
            // LINK: https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/generate-consume-asynchronous-stream
        }

        async Task<int> ICustomerRepository.Update(CustomerModel person)
        {
            if (person.Id != null)
            {
                _customerContext.Customers.Update(person);
                int updatedRows = await _customerContext.SaveChangesAsync();
                return updatedRows;
            }
            throw new ArgumentNullException(nameof(person));
        }
    }
}
