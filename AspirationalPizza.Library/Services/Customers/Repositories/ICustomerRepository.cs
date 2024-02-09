using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspirationalPizza.Library.Services.Customers.Repositories
{
    public interface ICustomerRepository
    {
        internal Task<int> Create(CustomerModel Person);
        internal Task<int> Update(CustomerModel Person);
        internal Task<int> Delete(CustomerModel Person);
        internal Task<List<CustomerModel>> Search(CustomerSearch Criteria);
        internal Task<CustomerModel?> Get(string PersonId);
    }
}
