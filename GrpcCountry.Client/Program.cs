using Grpc.Net.Client;
using GrpcCountryApi.Protos.v1;
using System;
using System.Threading.Tasks;

namespace GrpcCountry.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");

            var client = new CountryService.CountryServiceClient(channel);
            var reply =  client.GetAll(new EmptyRequest());
                                                                                                                                    
            //Console.WriteLine("Greeting: " + reply.Message);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
