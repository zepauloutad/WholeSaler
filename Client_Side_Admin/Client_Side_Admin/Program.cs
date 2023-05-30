using Grpc.Core;
using Server_Side;
using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Client_Side_Admin
{

    class Program
    {
        static async Task Main(string[] args)
        {

            var client = new Clients.TelecomClient();
            string choice = "";
            string username = "";
            string password = "";

            // Autenticação do Cliente

            try
            {
                Console.WriteLine("****************************************");
                Console.WriteLine("* Bem-Vindo ao Programa do Wholesaler! *");
                Console.WriteLine("****************************************");

                do
                {
                    Console.Write("Nome de Utilizador: ");
                    username = Console.ReadLine();
                    cBuffer();
                } while (username == null || username == "");
                do {
                    Console.Write("Password: ");
                    password = Console.ReadLine();
                    cBuffer();
                } while (password == null || password == "");

                var request = new UserCredentials
                {
                    Username = username,
                    Password = password
                };

                SqlConnection connection;
                string connectionString = "Data Source=DESKTOP-BB50T6N\\SQLEXPRESS;Initial Catalog=wholesaler;Integrated Security=True";
                connection = new SqlConnection(connectionString);
                connection.Open();

                username = request.Username;
                password = request.Password;

                string authQuery = "SELECT * FROM dbo.Clientes WHERE nome = @nome AND senha = @senha";

                SqlCommand command = new SqlCommand(authQuery, connection);
                command.Parameters.AddWithValue("@nome", username);
                command.Parameters.AddWithValue("@senha", password);

                // Create a response message based on the authentication result

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    Console.WriteLine("User authenticated...");
                }
                else
                {
                    Console.WriteLine("User not authenticated...");
                    Console.Read();
                    return;
                }

                connection.Close();

                Thread.Sleep(1000);
                Console.Clear();

            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR :: Invalid credentials!");
            }

            // Menu de Interface

            choice = iMenu();

            try
            {
                switch (choice)
                {
                    case "1":
                        Console.Write("Enter domicile: ");
                        var domicile = Console.ReadLine();
                        Console.Write("Enter mode: ");
                        var mode = Console.ReadLine();
                        client.ReserveDomicile(domicile, mode); 
                        break;
                    case "2":
                        Console.Write("Enter admin number: ");
                        var adminNumber2 = Console.ReadLine();
                        await client.ActivateAsync(adminNumber2);
                        break;
                    case "3":
                        Console.Write("Enter admin number: ");
                        var adminNumber3 = Console.ReadLine();
                        await client.DeactivateAsync(adminNumber3);
                        break;
                    case "4":
                        Console.Write("Enter admin number: ");
                        var adminNumber4 = Console.ReadLine();
                        await client.TerminateAsync(adminNumber4);
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                client.Shutdown();
            }
        }
        public static void cBuffer()
        {
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
        }
        public static string iMenu()
        {
            string uChoice = "";
            do
            {
                Console.WriteLine("******************************");
                Console.WriteLine("* What would you like to do? *");
                Console.WriteLine("******************************");
                Console.WriteLine("1 - Reserve");
                Console.WriteLine("2 - Activate");
                Console.WriteLine("3 - Deactivate");
                Console.WriteLine("4 - Terminate");

                uChoice = Console.ReadLine();
                cBuffer();
            } while (uChoice != "1" && uChoice != "2" && uChoice != "3" && uChoice != "4");

            return uChoice;
        }
    }
}
