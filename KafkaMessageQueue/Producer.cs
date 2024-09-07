using System;
using System.Collections.Generic;
using System.Linq;
using Confluent.Kafka;
using System;
using System.Threading.Tasks;

namespace KafkaMessageQueue;

public class Producer
{
    public static async Task ProduceMessage(string topic, string message)
    {
        var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

        using (var producer = new ProducerBuilder<Null, string>(config).Build())
        {
            try
            {
                var deliveryResult = await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
                Console.WriteLine($"Delivered '{deliveryResult.Value}' to '{deliveryResult.TopicPartitionOffset}'");
            }
            catch (ProduceException<Null, string> e)
            {
                Console.WriteLine($"Delivery failed: {e.Error.Reason}");
            }
        }
    }
}
