using ChatApplication.Code.Collections;
using ChatApplication.Models.Customer_Data;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChatApplication.Models.Customer_Data
{
    public class CustomerService
    {
        private readonly IMongoCollection<Customer> collection;
        public CustomerService(IOptions<CustomerCollection> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            var database = client.GetDatabase(options.Value.DatabaseName);
            //if (!database.ListCollectionNames().ToEnumerable().Contains(options.Value.CollectionName))
            //{
            //    database.CreateCollection(options.Value.CollectionName);
            //}
            this.collection = database.GetCollection<Customer>(options.Value.CollectionName);
        }

        public async Task<List<Customer>> GetAsync()
        {
            return await collection.Find(_ => true).ToListAsync();
        }
        public IQueryable<Customer> Get()
        {
            return collection.AsQueryable();
        }

        public List<string> GetCustomerTypes()
        {
            var customerTypes = (from c in collection.AsQueryable()
                                 select c.Type).Distinct();
            return customerTypes.ToList();
        }

        public Customer GetCustomerById(string id)
        {
            return collection.AsQueryable().FirstOrDefault(x => x.Id == id);
        }

    }
}
