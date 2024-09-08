using Confluent.Kafka;
using System.Collections.Concurrent;
using System.Reflection;
using ChatApplication.Code;
using ChatApplication.Models.Chat;

namespace ChatApplication.Services
{
    public class ConsumerManager
    {
        private readonly IConfiguration configuration;
        private readonly ConcurrentDictionary<string, IConsumer<Ignore, string>> consumers;
        private readonly ChatContext chatContext;

        public ConsumerManager(IConfiguration configuration, ChatContext chatContext)
        {
            this.configuration = configuration;
            this.consumers = new ConcurrentDictionary<string, IConsumer<Ignore, string>>();
            this.chatContext = chatContext;
        }

        public void CreateConsumer(string sender,string receiver)
        {
            string topic=GuidExtensions.GenerateUniqueGuid(sender,receiver).ToString();
            if (!consumers.ContainsKey(topic))
            {
                var config = new ConsumerConfig
                {
                    GroupId = "test-consumer-group",
                    BootstrapServers = configuration["Kafka:BootstrapServers"],
                    AutoOffsetReset = AutoOffsetReset.Earliest
                };

                var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
                consumers[topic] = consumer;

                Task.Run(() => Consume(topic, consumer));
            }
        }

        private void Consume(string topic, IConsumer<Ignore, string> consumer)
        {
            consumer.Subscribe(topic);
            try
            {
                while (true)
                {
                    var cr = consumer.Consume();
                    Console.WriteLine($"Consumed message '{cr.Message.Value}' at: '{cr.TopicPartitionOffset}' for topic '{topic}'.");
                }
            }
            catch (OperationCanceledException)
            {
                consumer.Close();
            }
        }

        public List<UserMessage> GetUserMessages(string user1,string user2)
        {
            string topic = GuidExtensions.GenerateUniqueGuid(user1, user2).ToString();
            return chatContext.GetMessages(topic);
        }

        public void StopConsumer(string sender, string receiver)
        {
            string topic = GuidExtensions.GenerateUniqueGuid(sender, receiver).ToString();
            if (consumers.TryRemove(topic, out var consumer))
            {
                consumer.Close();
            }
        }
    }
}
