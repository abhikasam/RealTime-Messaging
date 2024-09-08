using Confluent.Kafka;
using MongoDB.Bson.Serialization.IdGenerators;
using ChatApplication.Code;
using ChatApplication.Models.Chat;

namespace ChatApplication.Services
{
    public class ProducerService
    {
        private readonly IProducer<Null,string> producer;
        private readonly IConfiguration configuration;
        private readonly ILogger<ProducerService> logger;
        private readonly ChatContext chatContext;

        public ProducerService(IConfiguration configuration,ILogger<ProducerService> logger, ChatContext chatContext)
        {
            this.configuration = configuration;
            this.logger = logger;
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = configuration.GetValue<string>("Kafka:BootstrapServers")
            };
            producer = new ProducerBuilder<Null, string>(producerConfig).Build();
            this.chatContext = chatContext;
        }

        public async Task SendMessageAsync(string from,string to, string message)
        {
            try
            {
                var uniqueTopic = GuidExtensions.GenerateUniqueGuid(from, to).ToString();
                var deliveryResult = await producer.ProduceAsync(uniqueTopic, new Message<Null, string> { Value = message });
                var userMessage = new UserMessage()
                {
                    From=from,
                    To=to,
                    Topic=uniqueTopic,
                    Message=message,
                    TimeStamp=DateTime.Now
                };
                chatContext.AddMessage(userMessage);
                Console.WriteLine($"Delivered '{deliveryResult.Value}' to '{deliveryResult.TopicPartitionOffset}'.");
            }
            catch (ProduceException<Null, string> e)
            {
                Console.WriteLine($"Delivery failed: {e.Error.Reason}");
            }
        }
    }
}
