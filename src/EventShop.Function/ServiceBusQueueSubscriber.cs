using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace EventShop.Function;

public static class ServiceBusQueueSubscriber
{
    [FunctionName("ServiceBusQueueSubscriber")]
    public static void Run([ServiceBusTrigger("%QueueName%", Connection = "ServiceBusConnectionString")] ServiceBusReceivedMessage myQueueItem, ILogger log)
    {
        log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        // Add your message processing logic here
    }
}
