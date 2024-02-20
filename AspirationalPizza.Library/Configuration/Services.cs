
using AspirationalPizza.Library.Services.Customers;
using AspirationalPizza.Library.Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AspirationalPizza.Library.Services.Customers.Repositories;
using Microsoft.Extensions.Logging;

namespace AspirationalPizza.Library.Configuration
{
    public static class Services
    {
        public static void Configure(this IServiceCollection services, IConfiguration config)
        {

            /*  Design considration here -- we are registering the repo with the DI container and then injecting it to the constructor
             *  of the service.  If the service alone was responsible for the access pattern for the repo (which it should be) then we
             *  probably want to just have the service use the repo factory directly.  This would greatly simplify the need for
             *  customerConfig and we'd only need to register the injectable logger, config, and serviceprovider.  Mostly I'm doing this
             *  for academic reasons, I might take the other approach for non-customer services.
             */

            ServiceConfig<CustomerService> customerConfig = config.GetSection(ServiceKeys.Customers).Get<ServiceConfig<CustomerService>>() ??
                throw new ArgumentNullException("Customer Config section is not populated appropriately");    // Get the config section for our factory method
            services.Configure<ServiceConfig<CustomerService>>(config.GetSection(ServiceKeys.Customers));     // Add the config section for our service
            IRepository<CustomerModel> customerRepository = CustomerService.GetRepository(                           // Use the factory to create a repo
                services.BuildServiceProvider().GetRequiredService<ILogger<IRepository<CustomerModel>>>(),
                customerConfig);
            services.AddSingleton(customerRepository);                                                        // Register our repo on the DI Container
            services.AddScoped<ICustomerService, CustomerService>();                                          // Instantiate our service for the DI Container


            /*  Remember that time I tried to make this generic?
             * IRepository<CustomerModel> customerRepository = (IRepository<CustomerModel>)RepoSupport.RepoFactory
                .GetRepository<CustomerService, CustomerModel, CustomerDbContext, CustomerEFRepository, CustomerMongoRepository>(
                    services.BuildServiceProvider().GetRequiredService<ILogger<IRepository<CustomerModel>>>(), customerConfig); 
            */
        }
    }
}
