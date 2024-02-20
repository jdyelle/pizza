using AspirationalPizza.Library.Services.Customers.Repositories;

namespace AspirationalPizza.Library.Services.Customers
{
    public interface ICustomerService : IDisposable
    {
        Task<CustomerModel?> GetById(String CustomerId);
        Task<CustomerModel> CreateOrUpdate(CustomerModel customer);
        Task<List<CustomerModel>> BulkInsert(List<CustomerModel> customerList);
        Task<Boolean> Delete(CustomerModel customer);
        Task<List<CustomerModel>> Search(CustomerSearch criteria);

        CustomerModel DtoToModel(CustomerDto dto);
        CustomerDto ModelToDto(CustomerModel model);
    }
}
