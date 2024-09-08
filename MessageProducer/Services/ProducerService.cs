using Confluent.Kafka;

namespace MessageProducer.Services
{
    public class ProducerService
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<ProducerService> logger;
        private readonly IProducer<Null,string> producer;

        public ProducerService(IConfiguration configuration,ILogger<ProducerService> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
            var producerConfig = new ProducerConfig
            {
                BootstrapServers=configuration.GetValue<string>("Kafka:BootstrapServers")
            };
            producer = new ProducerBuilder<Null,string>(producerConfig).Build();
        }

        public async Task ProduceAsync(string topic,string message)
        {
            try
            {
                var result = await producer.ProduceAsync("messages", new Message<Null, string> { Value = message });
                logger.LogInformation($"Message sent to topic {result.Topic}, partition {result.Partition}, offset {result.Offset}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }

    }
}
