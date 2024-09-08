using Confluent.Kafka;
using System.Collections.Concurrent;
using System.Reflection;
using ChatApplication.Code;
using ChatApplication.Models.Chat;
using Microsoft.AspNetCore.SignalR;
using ChatApplication.Models.Customer_Data;

namespace ChatApplication.Services
{
    public class ConsumerManager
    {
        private readonly IConfiguration configuration;
        private readonly ConcurrentDictionary<string, IConsumer<Ignore, string>> consumers;
        private readonly ChatService chatContext;
        private readonly IHubContext<ChatHub> hubContext;
        private readonly CustomerService customerService;

        public ConsumerManager(IConfiguration configuration, ChatService chatContext, IHubContext<ChatHub> hubContext, CustomerService customerService)
        {
            this.configuration = configuration;
            this.consumers = new ConcurrentDictionary<string, IConsumer<Ignore, string>>();
            this.chatContext = chatContext;
            this.hubContext = hubContext;
            this.customerService = customerService;
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

                Task.Run(() => Consume(sender,receiver, consumer));
            }
        }

        private void Consume(string from,string to, IConsumer<Ignore, string> consumer)
        {
            string topic = GuidExtensions.GenerateUniqueGuid(from, to).ToString();
            consumer.Subscribe(topic);
            try
            {
                while (true)
                {
                    var cr = consumer.Consume();
                    var sender = customerService.GetCustomerById(from);
                    var receiver = customerService.GetCustomerById(to);
                    hubContext.Clients.All.SendAsync($"ReceiveMessage",sender,receiver,cr.Message.Value);
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
