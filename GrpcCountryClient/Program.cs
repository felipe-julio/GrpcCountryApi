using Grpc.Net.Client;
using System;

namespace GrpcCountry.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");

            //var client = new CountryService
            //var reply = await client.SayHelloAsync(new HelloRequest { Name = "GreeterClient" });

            //Console.WriteLine("Greeting: " + reply.Message);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
