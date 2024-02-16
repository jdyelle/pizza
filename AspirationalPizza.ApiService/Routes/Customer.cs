using AspirationalPizza.Library.Services.Customers;
using Microsoft.AspNetCore.Mvc;

namespace AspirationalPizza.ApiService.Routes
{
    public static class Customer
    {
        /* There's an argument that could be made here about putting these endpoints as a consumable thing in the service library,
         * but I feel like even having routes is a service consumption detail that shouldn't matter to the library at all, so I'm 
         * putting it here in the actual consumer of the service.  I don't see the point of having multiple APIs that point to the
         * same service funtionality (yet) -- to me that's in the scope of responsibility of the API project to manage.
         */
        internal static void AddEndpoints(this WebApplication app)
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
                    is CustomerModel created
                        ? Results.Ok(created)
                        : Results.NotFound());

            app.MapPut("/customers", async ([FromBody] CustomerDto[] customers, [FromServices] ICustomerService service) =>
                await service.CreateOrUpdate(service.DtoToModel(customers[0]))
                    is CustomerModel created
                        ? Results.Ok(created)
                        : Results.NotFound());

            app.MapGet("/customers/{id}", async ([FromRoute] string id, [FromServices] ICustomerService service) =>
                await service.GetById(id)
                    is CustomerModel customer
                        ? Results.Ok(service.ModelToDto(customer))
                        : Results.NotFound());

            app.MapDelete("/customers/{id}", async ([FromRoute] string id, [FromServices] ICustomerService service) =>
                await service.GetById(id)
                    is CustomerModel customer
                        ? Results.Ok(await service.Delete(customer))
                        : Results.NotFound());

            app.MapPut("/customers/{id}", async ([FromRoute] string id, [FromServices] ICustomerService service) =>
                await service.GetById(id)
                    is CustomerModel customer
                        ? Results.Ok(service.CreateOrUpdate(customer))
                        : Results.NotFound());

        }
    }
}
