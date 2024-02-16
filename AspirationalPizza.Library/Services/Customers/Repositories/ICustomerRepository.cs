using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspirationalPizza.Library.Services.Customers.Repositories
{
    public interface ICustomerRepository
    {
        internal Task<CustomerModel> Create(CustomerModel Customer);
        internal Task<CustomerModel> Update(CustomerModel Customer);
        internal Task<Boolean> Delete(CustomerModel Customer);
        internal Task<List<CustomerModel>> BulkInsert(List<CustomerModel> customerList);
        internal Task<List<CustomerModel>> Search(CustomerSearch Criteria);
        internal Task<CustomerModel?> Get(string CustomerId);
    }
}
