using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Client_Side_External_Operator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var client = new Clients.TelecomClient();
            string choice = "";
            string username = "";
            string password = "";
            int type = 0;
            string aux = "";

            // Autenticação do Cliente

            try
            {
                Console.WriteLine("****************************************");
                Console.WriteLine("* Bem-Vindo ao Programa do Wholesaler! *");
                Console.WriteLine("****************************************");

                do
                {
                    Console.Write("Tipo de Utilizador:\n(1) Operador\n(2) Operador Externo\n: ");
                    username = Console.ReadLine();
                    cBuffer();
                } while (type == null && type != 1 && type != 2);
                switch(type)
                {
                    case 1: aux = "Operador";break;
                    case 2: aux = "Operador_Externo";break;
                    default: break;
                }
                do
                {
                    Console.Write("Nome de Utilizador: ");
                    username = Console.ReadLine();
                    cBuffer();
                } while (username == null || username == "");
                do
                {
                    Console.Write("Password: ");
                    password = Console.ReadLine();
                    cBuffer();
                } while (password == null || password == "");

                SqlConnection connection;
                string connectionString = "Data Source=DESKTOP-BB50T6N\\SQLEXPRESS;Initial Catalog=wholesaler;Integrated Security=True";
                connection = new SqlConnection(connectionString);
                connection.Open();

                string authQuery = "SELECT * FROM dbo." + aux + " WHERE nome = @nome AND senha = @senha";

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
        }
        public static void cBuffer()
        {
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
        }
    }
}
