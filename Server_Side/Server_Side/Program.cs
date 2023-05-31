using System;
using Grpc.Core;
using Server_Side.Services;
using System.Data.SqlClient;
using static Server_Side.UserService;
using System.Threading.Tasks;

namespace Server_Side
{
    class Program
    {
        const int Port = 5001; 

        public static void Main(string[] args)
        {
            var reserveService = new ReserveService(); 
            var activateService = new ActivateService();
            var deactivateService = new DeactivateService();
            var terminateService = new TerminateService();

            var telecomService = new TelecomService(reserveService, activateService, deactivateService, terminateService);

            Server server = new Server
            {
                Services = { Telecom.BindService(telecomService) },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine($"Server listening on port {Port}");
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }
    }
}
