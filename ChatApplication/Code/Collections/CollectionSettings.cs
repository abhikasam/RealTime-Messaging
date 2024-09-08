namespace ChatApplication.Code.Collections
{
    public static class CollectionSettings
    {
        public static void AddCollections(this IServiceCollection services,IConfiguration configuration)
        {
            services.Configure<ChatCollection>(configuration.GetSection("MongoDb:chats"));
            services.Configure<CustomerCollection>(configuration.GetSection("MongoDb:customers"));
        }
    }
}
