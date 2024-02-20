namespace AspirationalPizza.Library.Services.Customers.Repositories
{
    public interface IRepository<CustomerModel> : IDisposable
    {
        Task<CustomerModel?> Get(string CustomerId);
        Task<CustomerModel> Create(CustomerModel Customer);
        Task<CustomerModel> Update(CustomerModel Customer);
        Task<Boolean> Delete(CustomerModel Customer);
        Task<List<CustomerModel>> BulkInsert(List<CustomerModel> customerList);
        Task<List<CustomerModel>> Search(CustomerSearch Criteria);
    }
}
