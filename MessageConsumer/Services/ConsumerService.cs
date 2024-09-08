
using Confluent.Kafka;

namespace MessageConsumer.Services
{
    public class ConsumerService : BackgroundService
    {
        private readonly IConsumer<Ignore,string> consumer;
        private readonly ILogger<ConsumerService> logger;
        private readonly string _bootstrapServers = "localhost:9092";
        private readonly string _topic = "messages";
        public ConsumerService(ILogger<ConsumerService> logger)
        {
            this.logger = logger;
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                GroupId = "user-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() =>
            {
                consumer.Subscribe(_topic);
                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var consumeResult = consumer.Consume(stoppingToken);
                        ProcessKafkaMessage(stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    consumer.Close();

                }
            }, stoppingToken);
        }

        private void ProcessKafkaMessage(CancellationToken stoppingToken)
        {
            try
            {
                var consumeResult = consumer.Consume(stoppingToken);
                var message = consumeResult.Message.Value;
                Console.WriteLine($"Message received : {message}");
                logger.LogInformation($"Message received : {message}");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error processing message : {ex.Message}");
            }
        }

    }
}
