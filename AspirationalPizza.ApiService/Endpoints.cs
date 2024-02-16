using AspirationalPizza.Library.Services.Customers;
using Microsoft.AspNetCore.Mvc;

namespace AspirationalPizza.ApiService
{
    public static class Endpoints
    {
        public static void Add(this WebApplication app)  //As the project gets more complex we can totally break this out to domains.
        {            
            Routes.Customer.AddEndpoints(app);                // Customer domain endpoints

        }
    }
}
