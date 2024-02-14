using AspirationalPizza.Library.Services.Customers;
using Microsoft.AspNetCore.Mvc;

namespace AspirationalPizza.ApiService
{
    public static class Routes
    {
        public static void Add(this WebApplication app)  //As the project gets more complex we can totally break this out to domains.
        {            
            AddCustomerEndpoints(app);                // Customer domain endpoints

        }

        private static void AddCustomerEndpoints(this WebApplication app)
        {
            app.MapGet("/customers", async ([FromServices] ICustomerService service) =>
            {
                List<CustomerModel> customers = await service.Search(new Library.Services.Customers.Repositories.CustomerSearch());
                List<CustomerDto> customersDto = new List<CustomerDto>();
                foreach (CustomerModel _customer in customers) customersDto.Add(service.ModelToDto(_customer));
                return Results.Ok(customersDto);
            });

            app.MapPost("/customers", async ([FromBody] CustomerDto customer, [FromServices] ICustomerService service) =>
            await service.CreateOrUpdate(service.DtoToModel(customer))
                is int rows
                    ? Results.Ok(rows)
                    : Results.NotFound());

            app.MapGet("/customers/{id}", async ([FromRoute] string id, [FromServices] ICustomerService service) =>
            await service.GetById(id)
                is CustomerModel customer
                    ? Results.Ok(service.ModelToDto(customer))
                    : Results.NotFound());
        }
    }
}
