using MongoDB.Bson.Serialization.Attributes;

namespace ChatApplication.Models.Chat
{
    public class UserMessage
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string _id { get; set; }
        public string From { get; set; }

        public string To { get; set; }
        public string Topic { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
