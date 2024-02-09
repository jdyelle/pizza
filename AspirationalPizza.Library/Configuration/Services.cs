using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspirationalPizza.Library.Services.Customers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ScarLibrary.Framework;

namespace AspirationalPizza.Library.Configuration
{
    public static class Services
    {
        public static IServiceCollection Configure(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<ServiceConfig<CustomerService>>(config.GetSection(ServiceKeys.Customers));

            return services;
        }
    }
}
