using Grpc.Net.Client;

namespace CalculatorClient
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // Disable TLS for MacOS
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            using var channel = GrpcChannel.ForAddress("http://localhost:5019");
            var client = new Calculator.CalculatorClient(channel);

            var addRequest = new AddRequest()
            {
                Number1 = 1,
                Number2 = 3
            };

            var result = client.Add(addRequest);
            
            Console.WriteLine(result.Sum);
        }
    }
}

