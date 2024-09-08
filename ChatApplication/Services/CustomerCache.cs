using ChatApplication.Models.Customer_Data;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace ChatApplication.Services
{
    public class CustomerCache
    {
        private readonly CustomerService customerService;
        private readonly ConnectionMultiplexer connectionMultiplexer;
        private readonly IDatabase database;

        public CustomerCache(IConfiguration configuration, CustomerService customerService)
        {
            this.customerService = customerService;
            connectionMultiplexer = ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis"));
            database = connectionMultiplexer.GetDatabase();
        }

        public IQueryable<Customer> GetCustomers()
        {
            var customers = database.ListRange("Customers", 0, -1);
            if (customers.Any())
            {
                return customers.Select(i => JsonConvert.DeserializeObject<Customer>(i.ToString())).OrderBy(i=>i._id).AsQueryable();
            }

            var customerList = customerService.Get();
            foreach (var customer in customerList)
            {
                string tmp = JsonConvert.SerializeObject(customer);
                database.ListLeftPush("Customers", tmp);
            }
            return customerList;
        }

    }
}
