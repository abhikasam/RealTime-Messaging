using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using ChatApplication.Code.Collections;

namespace ChatApplication.Models.Chat
{
    public class ChatContext
    {
        private readonly IMongoCollection<UserMessage> messages;
        public ChatContext(IOptions<ChatCollection> options)
        {
            var context = new MongoClient(options.Value.ConnectionString);
            var database = context.GetDatabase(options.Value.DatabaseName);
            if(!database.ListCollectionNames().ToEnumerable().Contains(options.Value.CollectionName)) 
            { 
                database.CreateCollection(options.Value.CollectionName);
            }
            messages = database.GetCollection<UserMessage>(options.Value.CollectionName);
        }


        public void AddMessage(UserMessage message)
        {
            messages.InsertOne(message);
        }

        public List<UserMessage> GetMessages(string topic)
        {
            return messages.Find(i=>i.Topic==topic).ToList();
        }


    }
}
