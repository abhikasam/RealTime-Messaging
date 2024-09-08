using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
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

		[BsonIgnore]
		public string FullName
		{
			get
			{
				return FirstName + " " + LastName;
			}
		}

    }
}

 