using System;
using Grpc.Core;
using Server_Side.Services;
using System.Data.SqlClient;

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

            SqlConnection connection;

            // TODO: Alterar a string de conexão para a final, (após começar a utilizar o Hamachi).
            // Autenticação -> Windows Authentication
            try
            {
                string connectionString = "Data Source=DESKTOP-BB50T6N\\SQLEXPRESS;Initial Catalog=wholesaler;Integrated Security=True";

                connection = new SqlConnection(connectionString);
                connection.Open();
                Console.WriteLine("Successfully connected to Database!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR :: Failed to connect to the Database!");
                return;
            }
            

            Server server = new Server
            {
                Services = { Telecom.BindService(telecomService) },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine($"Server listening on port {Port}");
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();
            connection.Close();

            server.ShutdownAsync().Wait();
        }
    }
}
