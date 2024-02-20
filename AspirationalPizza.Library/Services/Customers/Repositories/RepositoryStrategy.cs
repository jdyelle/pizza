using AspirationalPizza.Library.Services.FoodItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//TODO: Remove this dead code -- it's not used anywhere but was fun.
/*  This is an exploration of using the Strategy Pattern instead of using a case statement inside the factory.
 *  Ultimately I went with the case statement, the framework would have to do the same thing and switch is easier to follow.
 *  LINK: https://stackoverflow.com/questions/31950362/factory-method-with-di-and-ioc
 */
namespace AspirationalPizza.Library.Services.Customers.Repositories
{
    internal interface ICustomerRepositoryFactory
    {
        IRepository<CustomerModel> CreateRepository();
        bool AppliesTo(Type type);
    }

    internal interface ICustomerRepositoryStrategy
    {
        IRepository<CustomerModel> CreateRepository(Type type);
    }
}
