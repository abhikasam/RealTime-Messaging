using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace ChatApplication.Models.Customer_Data
{
    public partial class Customer
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string _id { get; set; }
		public string Id { get; set; }
		
		public string Type { get; set; }

		public string CustomerId { get; set; }
		public string Title { get; set; }
		public string FirstName { get; set; }
        public string LastName { get; set; }
		public string EmailAddress { get; set; }
		public string PhoneNumber { get; set; }
		public DateTime? CreationDate { get; set; }
		public DateTime? OrderDate { get; set; }
		public DateTime? ShipDate { get; set; }
		[BsonIgnoreIfNull]
		public ICollection<Address> Addresses { get; set; }
		[BsonIgnoreIfNull]
		public ICollection<ProductItem> Details { get; set; }
		public int? SalesOrderCount { get; set; }
    }
}

/*
{
"id" : "0012D555-C7DE-4C4B-B4A4-2E8A6B8E1161",
	"type" : "customer",
	"customerId" : "0012D555-C7DE-4C4B-B4A4-2E8A6B8E1161",
	"title" : "",
	"firstName" : "Franklin",
	"lastName" : "Ye",
	"emailAddress" : "franklin9@adventure-works.com",
	"phoneNumber" : "1 (11) 500 555-0139",
	"creationDate" : "2014-02-05T00:00:00",
	"addresses" : [
		{
			"addressLine1" : "1796 Westbury Dr.",
			"addressLine2" : "",
			"city" : "Melton",
			"state" : "VIC",
			"country" : "AU",
			"zipCode" : "3337"
		}
	],
	"password" : {
    "hash" : "GQF7qjEgMl3LUppoPfDDnPtHp1tXmhQBw0GboOjB8bk=",
		"salt" : "12C0F5A5"

    },
	"salesOrderCount" : 2

} 
 */