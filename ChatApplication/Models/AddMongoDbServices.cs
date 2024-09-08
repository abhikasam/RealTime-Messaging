using ChatApplication.Models.Chat;
using ChatApplication.Models.Customer_Data;

namespace ChatApplication.Models
{
    public static class AddMongoDbServices
    {
        public static void AddMongoDbService(this IServiceCollection services)
        {
            services.AddScoped<ChatService>();
            services.AddScoped<CustomerService>();
        }
    }
}
