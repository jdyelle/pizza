using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspirationalPizza.Library.Services.Customers;
using AspirationalPizza.Library.Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AspirationalPizza.Library.Services.Customers.Repositories;
using Microsoft.Extensions.Logging;

namespace AspirationalPizza.Library.Configuration
{
    public static class Services
    {
        public static IServiceCollection Configure(this IServiceCollection services, IConfiguration config)
        {

            /*  Design considration here -- we are registering the repo with the DI container and then injecting it to the constructor
             *  of the service.  If the service alone was responsible for the access pattern for the repo (which it should be) then we
             *  probably want to just have the service use the repo factory directly.  This would greatly simplify the need for
             *  customerConfig and we'd only need to register the injectable logger, config, and servicebinder.  Mostly I'm doing this
             *  for academic reasons, I might take the other approach for non-customer services.
             */
            ServiceConfig<CustomerService> customerConfig = config.GetSection(ServiceKeys.Customers).Get<ServiceConfig<CustomerService>>() ??
                throw new ArgumentNullException("Customer Config section is not populated appropriately");  // Get the config section for our factory method
            services.Configure<ServiceConfig<CustomerService>>(config.GetSection(ServiceKeys.Customers));   // Add the config section for our service
            ICustomerRepository customerRepository = CustomerService.GetRepository(                         // Use the factory to create a repo
                services.BuildServiceProvider().GetRequiredService<ILogger<ICustomerRepository>>(), 
                customerConfig);
            services.AddSingleton(customerRepository);                                                      // Register our repo on the DI Container
            services.AddKeyedScoped<ICustomerService, ICustomerService>(ServiceKeys.Customers);             // Instantiate our service for the DI Container


            return services;
        }
    }
}
