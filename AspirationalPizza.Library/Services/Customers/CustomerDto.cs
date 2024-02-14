using AutoMapper;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspirationalPizza.Library.Services.Customers
{
    public record CustomerDto
    {
        public String? Id { get; set; }
        public String? FirstName { get; set; } = null;
        public String? LastName { get; set; } = null;
        public List<String> Emails { get; set; } = new List<String>();
        public List<String> PhoneNumbers { get; set; } = new List<String>();
        public List<CustomerAddressDto> CustomerAddresses { get; set; } = new List<CustomerAddressDto>();
        public List<String> FavoriteFoodItems { get; set; } = new List<String>();
    }

    public record CustomerAddressDto
    {
        public String StreetAddress { get; set; } = String.Empty;
        public String City { get; set; } = String.Empty;
        public String State { get; set; } = String.Empty;
        public String ZipCode { get; set; } = String.Empty;
    }

}
