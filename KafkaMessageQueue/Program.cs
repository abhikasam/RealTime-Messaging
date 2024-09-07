// See https://aka.ms/new-console-template for more information
using KafkaMessageQueue;
using System.Threading;

string topic = "test-topic";

// Produce a message
var cancellationTokenSource = new CancellationTokenSource();
Task.Run(() => Consumer.ConsumeMessages(topic, cancellationTokenSource.Token));

while (true)
{
    var read = Console.ReadLine();
    if (string.IsNullOrEmpty(read))
    {
        cancellationTokenSource.Cancel();
        break;
    }

    await Producer.ProduceMessage(topic, read);
}