using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspirationalPizza.Library.Services.Customers.Repositories;

namespace AspirationalPizza.Library.Services.Customers
{
    public interface ICustomerService : IDisposable
    {
        Task<CustomerModel?> GetById(String id);
        Task<CustomerModel> CreateOrUpdate(CustomerModel customer);
        Task<List<CustomerModel>> BulkInsert(List<CustomerModel> customerList);
        Task<Boolean> Delete(CustomerModel newPerson);
        Task<List<CustomerModel>> Search(CustomerSearch criteria);

        CustomerModel DtoToModel(CustomerDto dto);
        CustomerDto ModelToDto(CustomerModel model);
    }
}
