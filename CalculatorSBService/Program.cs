// See https://aka.ms/new-console-template for more information

using System.Text;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CalculatorSBService;
internal class Program
{
    static async Task Main(string[] args)
    {
        using IHost host = Host.CreateDefaultBuilder(args).UseEnvironment("Development").Build();
        IConfiguration config = host.Services.GetRequiredService<IConfiguration>();
        var connection = config.GetValue<string>("connection");
        
        await using var client = new ServiceBusClient(connection);
        var options = new ServiceBusReceiverOptions()
        {
            ReceiveMode = ServiceBusReceiveMode.PeekLock
        };
        var receiver = client.CreateReceiver("queue-1");

        while (true)
        {
            var message = await receiver.ReceiveMessageAsync();
            if (message == null)
            {
                Console.WriteLine("No available messages");
                await Task.Delay(100);
                continue;
            }
            
            Console.WriteLine("Received: " + message.MessageId);

            var body = message.Body;
            if (body != null)
            {
                var json = Encoding.UTF8.GetString(body);
                var numbers = System.Text.Json.JsonSerializer.Deserialize<Numbers>(json);
            
                await receiver.CompleteMessageAsync(message);
            }
            else
            {
                await receiver.DeadLetterMessageAsync(message);
            }
        }
    }
}

public class Numbers
{
    public double Number1 { get; set; }
    public double Number2 { get; set; }
}