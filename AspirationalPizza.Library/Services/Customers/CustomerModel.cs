using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace AspirationalPizza.Library.Services.Customers
{
    public record CustomerModel
    {
        [Key]
        [BsonId]
        public String? CustomerId { get; set; }        
        public String? FirstName { get; set; } = null;
        public String? LastName { get; set; } = null;
        public List<String> Emails { get; set; } = new List<String>();
        public List<String> PhoneNumbers { get; set; } = new List<String>();
        public List<CustomerAddress> CustomerAddresses { get; set; } = new List<CustomerAddress>();
        public List<String> FavoriteFoodItems { get; set; } = new List<String>();
    }

    public record CustomerAddress
    {
        [Key]
        public String? AddressId { get; set; } = null; // This is just for EF to do its thing.
        public String StreetAddress { get; set; } = String.Empty;
        public String City { get; set; } = String.Empty;
        public String State { get; set; } = String.Empty;
        public String ZipCode { get; set; } = String.Empty;
    }
}
