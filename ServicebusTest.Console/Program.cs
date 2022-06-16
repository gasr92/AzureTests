using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;


IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var conn = config["conn"];



// remove the entitypath from connection string if exists
// generate SAS in the queue
var connString = config["Servicebus:conn"];
var queueName = config["Servicebus:queueName"];

ServiceBusClient client = new ServiceBusClient(connString);
ServiceBusSender sender = client.CreateSender(queueName);

Console.WriteLine("Write something:");
var word = Console.ReadLine();

try
{
    for (int i = 0; i < 1; i++)
    {
        var message = new ServiceBusMessage($"{i.ToString()}: {word}");
        await sender.SendMessageAsync(message);
        Console.WriteLine("Mensagem enviada");
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}