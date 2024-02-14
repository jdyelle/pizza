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
        public override Dictionary<string, string> Attributes
        {
            get
            {
                return new Dictionary<string, string>
                    {
                        { "Id", "String" },
                        { "Names.FirstName", "String" },
                        { "Names.LastName", "String" },
                        { "Identifiers.Emails", "String[]" },
                        { "Identifiers.PhoneNumbers", "String[]" },
                        { "Addresses.Street", "String" },
                        { "Addresses.City", "String" },
                        { "Addresses.State", "DateTime" },
                        { "Addresses.ZipCode", "Number" }
                    };
            }
        }
    }
}
