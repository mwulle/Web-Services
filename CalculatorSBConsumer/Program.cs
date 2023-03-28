// See https://aka.ms/new-console-template for more information

using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CalculatorSBConsumer;

internal class Program
{
    static async Task Main(string[] args)
    {
        using IHost host = Host.CreateDefaultBuilder(args).UseEnvironment("Development").Build();
        IConfiguration config = host.Services.GetRequiredService<IConfiguration>();
        var connection = config.GetValue<string>("connection");
        
        await using var client = new ServiceBusClient(connection);
        await using var sender = client.CreateSender("queue-1");

        var numbers = new Numbers { Number1 = 1, Number2 = 2 };
        var json = System.Text.Json.JsonSerializer.Serialize(numbers);
        var body = System.Text.Encoding.UTF8.GetBytes(json);

        var message = new ServiceBusMessage(body);
        message.CorrelationId = Guid.NewGuid().ToString();
        
        await sender.SendMessageAsync(message);
        Console.WriteLine("Message send");
    }
}

public class Numbers
{
    public double Number1 { get; set; }
    public double Number2 { get; set; }
}