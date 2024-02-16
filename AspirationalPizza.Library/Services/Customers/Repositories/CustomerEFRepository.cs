using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspirationalPizza.Library.RepoSupport;
using System.ComponentModel.DataAnnotations.Schema;

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

        async Task<CustomerModel> ICustomerRepository.Create(CustomerModel customer)
        {
            if (customer.CustomerId != null)
            {
                _customerContext.Customers.Add(customer);
                int updatedRows = await _customerContext.SaveChangesAsync();
                if (updatedRows > 0) return customer;
                throw new InvalidDataException("Unable to create new customer record ");
            }
            throw new ArgumentNullException(nameof(customer));
        }

        async Task<Boolean> ICustomerRepository.Delete(CustomerModel customer)
        {
            _customerContext.Remove(customer);
            int updatedRows = await _customerContext.SaveChangesAsync();
            if (updatedRows > 0) return true;
            throw new InvalidDataException("Unable to delete specified customer record ");
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

        async Task<CustomerModel> ICustomerRepository.Update(CustomerModel customer)
        {
            if (customer.CustomerId != null)
            {
                _customerContext.Customers.Update(customer);
                int updatedRows = await _customerContext.SaveChangesAsync();
                if (updatedRows > 0) return customer;
            }
            throw new ArgumentNullException(nameof(customer));
        }

        async Task<List<CustomerModel>> ICustomerRepository.BulkInsert(List<CustomerModel> customers)
        {
            List<EntityEntry<CustomerModel>> confirmed = new List<EntityEntry<CustomerModel>>();
            List<CustomerModel> returnList = new List<CustomerModel>();
            foreach (CustomerModel _customer in customers)
                if (_customer.CustomerId != null) 
                    confirmed.Add(_customerContext.Customers.Add(_customer));

            int updatedRows = await _customerContext.SaveChangesAsync();

            if (updatedRows > 0) 
                foreach (EntityEntry<CustomerModel> _entry in confirmed) 
                    if (_entry.State.HasFlag(EntityState.Unchanged)) 
                        returnList.Add(_entry.Entity);

            if (customers.Count != returnList.Count) _logger
                    .LogWarning("Not all customer entries were added to the database from the bulk insert.");

            return returnList;
        }
    }
}
