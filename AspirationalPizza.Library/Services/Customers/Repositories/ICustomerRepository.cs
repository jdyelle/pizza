using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspirationalPizza.Library.Services.Customers.Repositories
{
    public interface ICustomerRepository
    {
        internal Task<int> Create(CustomerModel Customer);
        internal Task<int> Update(CustomerModel Customer);
        internal Task<int> Delete(CustomerModel Customer);
        internal Task<List<CustomerModel>> Search(CustomerSearch Criteria);
        internal Task<CustomerModel?> Get(string CustomerId);
    }
}
