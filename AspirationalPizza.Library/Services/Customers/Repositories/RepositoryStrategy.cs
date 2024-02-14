using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspirationalPizza.Library.Services.Customers.Repositories
{
    internal interface ICustomerRepositoryFactory
    {
        ICustomerRepository CreateRepository();
        bool AppliesTo(Type type);
    }

    internal interface ICustomerRepositoryStrategy
    {
        ICustomerRepository CreateRepository(Type type);
    }
}
