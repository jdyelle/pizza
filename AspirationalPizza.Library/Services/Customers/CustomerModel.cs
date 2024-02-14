using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AspirationalPizza.Library.Services.Customers
{
    public record CustomerModel
    {
        public String? Id { get; set; }
        
        public String? FirstName { get; set; } = null;
        public String? LastName { get; set; } = null;
        public List<String> Emails { get; set; } = new List<String>();
        public List<String> PhoneNumbers { get; set; } = new List<String>();
        public List<CustomerAddress> CustomerAddresses { get; set; } = new List<CustomerAddress>();
        public List<String> FavoriteFoodItems { get; set; } = new List<String>();

    }

    public record CustomerAddress
    {
        public String StreetAddress { get; set; } = String.Empty;
        public String City { get; set; } = String.Empty;
        public String State { get; set; } = String.Empty;
        public String ZipCode { get; set; } = String.Empty;
    }
}
