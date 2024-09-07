using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KafkaMessageQueue
{
    public class Consumer
    {
        public static void ConsumeMessages(string topic, CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = "test-consumer-group",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe(topic);

                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var consumeResult = consumer.Consume(cancellationToken);
                        Console.WriteLine($"Received message: {consumeResult.Message.Value}");
                    }
                }
                catch (OperationCanceledException)
                {
                    consumer.Close();
                }
            }
        }
    }
}
