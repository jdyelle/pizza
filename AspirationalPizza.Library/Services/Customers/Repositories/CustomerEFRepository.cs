using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using AspirationalPizza.Library.RepoSupport;

namespace AspirationalPizza.Library.Services.Customers.Repositories
{
    internal class CustomerEFRepository : IRepository<CustomerModel>
    {
        private readonly CustomerDbContext _customerContext;
        private readonly ILogger<IRepository<CustomerModel>> _logger;

        public CustomerEFRepository(ILogger<IRepository<CustomerModel>> logger, CustomerDbContext customerContext)
        {
            _customerContext = customerContext;
            _logger = logger;
        }

        async Task<CustomerModel> IRepository<CustomerModel>.Create(CustomerModel customer)
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

        async Task<Boolean> IRepository<CustomerModel>.Delete(CustomerModel customer)
        {
            _customerContext.Remove(customer);
            int updatedRows = await _customerContext.SaveChangesAsync();
            if (updatedRows > 0) return true;
            throw new InvalidDataException("Unable to delete specified customer record ");
        }

        async Task<CustomerModel?> IRepository<CustomerModel>.Get(string customerId)
        {
            CustomerModel? got = await _customerContext.Customers.Where(c => c.CustomerId == customerId)
                .Include(p => p.CustomerAddresses)
                .FirstOrDefaultAsync();
            return got;
        }

        async Task<List<CustomerModel>> IRepository<CustomerModel>.Search(CustomerSearch searchObject)
        {
            //Not sure why the ToListAsync method doesn't show up here yet, but I'll live with the warning for now.
            return _customerContext.Customers.Where(LinqSearch<CustomerModel>.FilterBuilder(searchObject)).ToList();

            //Here are some notes for further reading:
            // LINK: https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/generate-consume-asynchronous-stream
        }

        async Task<CustomerModel> IRepository<CustomerModel>.Update(CustomerModel customer)
        {
            if (customer.CustomerId != null)
            {
                _customerContext.Customers.Update(customer);
                int updatedRows = await _customerContext.SaveChangesAsync();
                if (updatedRows > 0) return customer;
            }
            throw new ArgumentNullException(nameof(customer));
        }

        async Task<List<CustomerModel>> IRepository<CustomerModel>.BulkInsert(List<CustomerModel> customers)
        {
            List<EntityEntry<CustomerModel>> confirmed = new List<EntityEntry<CustomerModel>>();
            List<CustomerModel> returnList = new List<CustomerModel>();
            foreach (CustomerModel _customer in customers)
            {
                if (String.IsNullOrEmpty(_customer.CustomerId)) _customer.CustomerId = Guid.NewGuid().ToString();
                confirmed.Add(_customerContext.Customers.Add(_customer));
            }

            int updatedRows = await _customerContext.SaveChangesAsync();

            if (updatedRows > 0) 
                foreach (EntityEntry<CustomerModel> _entry in confirmed) 
                    if (_entry.State.HasFlag(EntityState.Unchanged)) 
                        returnList.Add(_entry.Entity);

            if (customers.Count != returnList.Count) _logger
                    .LogWarning("Not all customer entries were added to the database from the bulk insert.");

            return returnList;
        }

        public void Dispose() { _customerContext.Dispose(); }
    }
}
