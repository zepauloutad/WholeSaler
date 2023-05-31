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
            int type = 0;
            string userType = "";

            SqlConnection connection;
            string connectionString = "Data Source=DESKTOP-R5A13LN\\SQLEXPRESS;Initial Catalog=wholesaler;Integrated Security=True";
            connection = new SqlConnection(connectionString);
            connection.Open();

            // Autenticação do Cliente

            try
            {
                Console.WriteLine("****************************************");
                Console.WriteLine("* Bem-Vindo ao Programa do Wholesaler! *");
                Console.WriteLine("****************************************");

                do
                {
                    Console.Write("Tipo de Utilizador:\n(0) Administrador\n(1) Operador\n(2) Operador Externo\n: ");
                    username = Console.ReadLine();
                    cBuffer();
                } while (type == null && type != 0 && type != 1 && type != 2);
                switch (type)
                {
                    case 0: userType = "Administrador"; break;
                    case 1: userType = "Operador"; break;
                    case 2: userType = "Operador_Externo"; break;
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

                

                string authQuery = "SELECT * FROM dbo." + userType + " WHERE nome = @nome AND senha = @senha";

                SqlCommand userLogin = new SqlCommand(authQuery, connection);
                userLogin.Parameters.AddWithValue("@nome", username);
                userLogin.Parameters.AddWithValue("@senha", password);

                // Create a response message based on the authentication result

                SqlDataReader reader = userLogin.ExecuteReader();

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

                reader.Close();
                Thread.Sleep(1000);
                Console.Clear();

            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR :: Invalid credentials!");
            }

            // Caso o Utilizador seja do tipo Administrador:

            if (userType == "Administrador")
            {
                adminMenu(connection);
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
                connection.Close();
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
        public static void adminMenu(SqlConnection connection)
        {
            string uChoice = "";
            Console.WriteLine("******************************");
            Console.WriteLine("* What would you like to do? *");
            Console.WriteLine("******************************");
            Console.WriteLine("1 - Change the data of a Operador");
            Console.WriteLine("2 - Change the data of a External Operador");
            Console.Write(": ");
            uChoice = Console.ReadLine();
            cBuffer();
            switch (uChoice)
            {
                case "1": editOperador(connection);break;
                case "2": editExternalOperador(connection);break;
                default: Console.Write("Invalido"); break;
            }
        }
        public static void editOperador(SqlConnection connection)
        {
            string id = "";
            string op = "";
            Console.Write("Operador's ID: ");
            id = Console.ReadLine();
            cBuffer();
            Console.WriteLine("Choose an option:");
            Console.WriteLine("(0) Change the Username of the Operador");
            Console.WriteLine("(1) Change the Password of the Operador");
            Console.WriteLine("(2) Delete Operador from the Database");
            Console.Write(": ");
            op = Console.ReadLine();
            cBuffer();
            switch (op)
            {
                // Alterar nome de utilizador de operador
                case "0": 
                    Console.Write("Please specify the new Operador's Username: ");
                    string newUsername = Console.ReadLine();
                    cBuffer();
                    try
                    {
                        // SQL para alterar os dados do operário
                        string updateQuery = "UPDATE dbo.Operador SET nome = @nome WHERE id_operador = @id";

                        SqlCommand updateOperario = new SqlCommand(updateQuery, connection);
                        updateOperario.Parameters.AddWithValue("@id", id);
                        updateOperario.Parameters.AddWithValue("@nome", newUsername);

                        int rowsAffected = updateOperario.ExecuteNonQuery();

                        Console.WriteLine("Operario successfully updated!");
                        Console.WriteLine(rowsAffected + " row(s) updated");

                        Console.Read();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ERROR :: Invalid Operador's ID!");
                    }
                    break;
                // Alterar password de operador
                case "1": 
                    Console.Write("Please specify the new Operador's Password: ");
                    string newPassword = Console.ReadLine();
                    cBuffer();
                    try
                    {
                        // SQL para alterar os dados do operário
                        string updateQuery = "UPDATE dbo.Operador SET senha = @senha WHERE id_operador = @id";

                        SqlCommand updateOperario = new SqlCommand(updateQuery, connection);
                        updateOperario.Parameters.AddWithValue("@id", id);
                        updateOperario.Parameters.AddWithValue("@senha", newPassword);

                        int rowsAffected = updateOperario.ExecuteNonQuery();

                        Console.WriteLine("Operario successfully updated!");
                        Console.WriteLine(rowsAffected + " row(s) updated");

                        Console.Read();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ERROR :: Invalid Operador's ID!");
                    }
                    break;
                // Eliminar operador
                case "2": 
                    try
                    {
                        // SQL para eliminar os dados do operário
                        string deleteQuery = "DELETE FROM dbo.Operador WHERE id_operador = @id";

                        SqlCommand deleteOperario = new SqlCommand(deleteQuery, connection);
                        deleteOperario.Parameters.AddWithValue("@id", id);

                        int rowsAffected = deleteOperario.ExecuteNonQuery();

                        Console.WriteLine("Operario successfully deleted!");
                        Console.WriteLine(rowsAffected + " row(s) deleted");
                        Console.Read();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ERROR :: Invalid Operador's ID!");
                    }
                    break;

                default: break;
            }
        }
        public static void editExternalOperador(SqlConnection connection)
        {
            string id = "";
            string op = "";
            Console.Write("Operador's ID: ");
            id = Console.ReadLine();
            cBuffer();
            Console.WriteLine("Choose an option:");
            Console.WriteLine("(0) Change the Username of the Operador");
            Console.WriteLine("(1) Change the Password of the Operador");
            Console.WriteLine("(2) Delete Operador from the Database");
            Console.Write(": ");
            op = Console.ReadLine();
            cBuffer();
            switch (op)
            {
                // Alterar nome de utilizador de operador
                case "0": 
                    Console.Write("Please specify the new External Operador's Username: ");
                    string newUsername = Console.ReadLine();
                    cBuffer();
                    try
                    {
                        // SQL para alterar os dados do operário
                        string updateQuery = "UPDATE dbo.Operador_Externo SET nome = @nome WHERE id_operador = @id";

                        SqlCommand updateOperario = new SqlCommand(updateQuery, connection);
                        updateOperario.Parameters.AddWithValue("@id", id);
                        updateOperario.Parameters.AddWithValue("@nome", newUsername);

                        int rowsAffected = updateOperario.ExecuteNonQuery();

                        Console.WriteLine("Operario successfully updated!");
                        Console.WriteLine(rowsAffected + " row(s) updated");

                        Console.Read();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ERROR :: Invalid External Operador's ID!");
                    }
                    break;
                // Alterar password de operador
                case "1": 
                    Console.Write("Please specify the new External Operador's Password: ");
                    string newPassword = Console.ReadLine();
                    cBuffer();
                    try
                    {
                        // SQL para alterar os dados do operário
                        string updateQuery = "UPDATE dbo.Operador_Externo SET senha = @senha WHERE id_operador = @id";

                        SqlCommand updateOperario = new SqlCommand(updateQuery, connection);
                        updateOperario.Parameters.AddWithValue("@id", id);
                        updateOperario.Parameters.AddWithValue("@senha", newPassword);

                        int rowsAffected = updateOperario.ExecuteNonQuery();

                        Console.WriteLine("Operario successfully updated!");
                        Console.WriteLine(rowsAffected + " row(s) updated");

                        Console.Read();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ERROR :: Invalid External Operador's ID!");
                    }
                    break;
                // Eliminar operador
                case "2": 
                    try
                    {
                        // SQL para eliminar os dados do operário
                        string deleteQuery = "DELETE FROM dbo.Operador_Externo WHERE id_operador = @id";

                        SqlCommand deleteOperario = new SqlCommand(deleteQuery, connection);
                        deleteOperario.Parameters.AddWithValue("@id", id);

                        int rowsAffected = deleteOperario.ExecuteNonQuery();

                        Console.WriteLine("Operario successfully deleted!");
                        Console.WriteLine(rowsAffected + " row(s) deleted");
                        Console.Read();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ERROR :: Invalid External Operador's ID!");
                    }
                    break;

                default: break;
            }
        }
    }
}
