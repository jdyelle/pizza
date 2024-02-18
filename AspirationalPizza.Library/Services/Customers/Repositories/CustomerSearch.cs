using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspirationalPizza.Library.RepoSupport;

namespace AspirationalPizza.Library.Services.Customers.Repositories
{
    public class CustomerSearch : SearchBase
    {
        public CustomerSearch() : base() { }


        /// <summary> Pass this to a new SearchCriteria as valid attributes and types to build a filter.
        /// These are type specific filter validations. </summary>
        public override Dictionary<string, string> AttributeMeta
        {
            get
            {
                return new Dictionary<string, string>
                    {
                        { "Id", "String" },
                        { "FirstName", "String" },
                        { "LastName", "String" },
                        { "Emails", "String[]" },
                        { "PhoneNumbers", "String[]" },
                        { "Addresses.Street", "String" },
                        { "Addresses.City", "String" },
                        { "Addresses.State", "DateTime" },
                        { "Addresses.ZipCode", "Number" }
                    };
            }
        }

        new public record Attributes : SearchBase.Attributes
        { 
            public const string Id = "Id";
            public const string FirstName = "FirstName";
            public const string LastName = "FirstName";
            public const string Emails = "Emails";
            public const string PhoneNumbers = "PhoneNumbers";
            public const string AddressesStreet = "Addresses.Street";
            public const string AddressesCity = "Addresses.City";
            public const string AddressesState = "Addresses.State";
            public const string AddressesZipCode = "Addresses.ZipCode";
        }
    }
}
