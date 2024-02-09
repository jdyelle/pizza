﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspirationalPizza.Library.Services.Customers.Repositories;

namespace AspirationalPizza.Library.Services.Customers
{
    internal interface ICustomerService
    {
        Task<CustomerModel?> GetById(String id);
        Task<int> CreateOrUpdate(CustomerModel person);
        Task<int> Delete(CustomerModel newPerson);
        Task<List<CustomerModel>> Search(CustomerSearch criteria);
    }
}