using AspirationalPizza.Library.Configuration;
using AspirationalPizza.Library.Services.Customers.Repositories;
using AspirationalPizza.Library.Services.Customers;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Tests.Library.Services
{
    [TestClass]
    public class Customers
    {
        private static ICustomerService _customerService = null!;

        [ClassInitialize]
        public static void SetupService(TestContext context)
        {
            ILogger<ICustomerRepository> repoLogger = new NullLogger<ICustomerRepository>();
            ILogger<CustomerService> serviceLogger = new NullLogger<CustomerService>();

            ServiceConfig<CustomerService> config = new AspirationalPizza.Library.Configuration.ServiceConfig<CustomerService>
            {
                Repository = new AspirationalPizza.Library.Configuration.RepoConfig
                { RepositoryType = "Memory" }
            };

            ICustomerRepository repo = CustomerService.GetRepository(repoLogger, config);
            _customerService = new CustomerService(serviceLogger, repo, Options.Create(config));

        }

        [ClassCleanup]
        public static void CleanupService()
        {
            _customerService.Dispose();
        }

        [TestMethod]
        public void Create_Get_Delete_Customer()
        {
            CustomerModel customer = new CustomerModel
            {
                FirstName = "Jeremy",
                LastName = "Yelle",
                PhoneNumbers = new List<string> { "585-867-5309", "800-555-1234"},
                CustomerAddresses = new List<CustomerAddress> { new CustomerAddress
                {
                    StreetAddress = "123 Main Street",
                    City = "Rochester",
                    State = "NY",
                    ZipCode = "14627"
                } },
                Emails = new List<string> { "jdyelle@github.com" }
            };


            CustomerModel createCustomer = _customerService.CreateOrUpdate(customer).Result;
            Assert.IsNotNull(createCustomer.CustomerId);

            CustomerModel getCustomer = _customerService.GetById(createCustomer.CustomerId).Result!;

            Assert.AreEqual(customer.FirstName, getCustomer.FirstName);
            Assert.AreEqual(customer.LastName, getCustomer.LastName);
            Assert.AreEqual(customer.Emails, getCustomer.Emails);
            Assert.AreEqual(customer.PhoneNumbers, getCustomer.PhoneNumbers);
            Assert.AreEqual(customer.CustomerAddresses, getCustomer.CustomerAddresses);

            Boolean deleted = _customerService.Delete(getCustomer).Result;
            CustomerModel? getDeleted = _customerService.GetById(createCustomer.CustomerId).Result;

            Assert.IsNull(getDeleted);
        }

        [TestMethod]
        public void BulkInsert_Search_Customer()
        {
            CustomerModel luke = new CustomerModel
            {
                FirstName = "Luke",
                LastName = "Skywaler",
                PhoneNumbers = new List<string> { "111-222-3333", "123-456-7890" },
                CustomerAddresses = new List<CustomerAddress> { new CustomerAddress
                {
                    StreetAddress = "123 Tattooine Highway",
                    City = "Lars Farm",
                    State = "Tattooine",
                    ZipCode = "11111"
                } },
                Emails = new List<string> { "luke@thirsty.org" }
            };

            CustomerModel leia = new CustomerModel
            {
                FirstName = "Leia",
                LastName = "Organa",
                PhoneNumbers = new List<string> { "222-333-4444", "123-456-7890" },
                CustomerAddresses = new List<CustomerAddress> { new CustomerAddress
                {
                    StreetAddress = "456 Alderaan Overpass",
                    City = "Royal Palace",
                    State = "Alderaan",
                    ZipCode = "22222"
                } },
                Emails = new List<string> { "leia@princess.gov" }
            };

            CustomerModel vader = new CustomerModel
            {
                FirstName = "Darth",
                LastName = "Vader",
                PhoneNumbers = new List<string> { "333-444-5555" },
                CustomerAddresses = new List<CustomerAddress> { new CustomerAddress
                {
                    StreetAddress = "789 Death Star Circle",
                    City = "NotAMoon",
                    State = "Empire",
                    ZipCode = "33333"
                } },
                Emails = new List<string> { "vader@spacestation.com" }
            };


            List<CustomerModel> inserted = _customerService.BulkInsert(new List<CustomerModel> { luke, leia, vader }).Result;
            Assert.AreEqual(inserted.Count(), 3);

            CustomerSearch criteria = new CustomerSearch();
            criteria.AddFilter(CustomerSearch.Attributes.PhoneNumbers, CustomerSearch.Comparisons.Equal, "123-456-7890");
            List<CustomerModel> searchResult = _customerService.Search(criteria).Result;

            Assert.AreEqual(searchResult.Count, 2);

        }
    }
}