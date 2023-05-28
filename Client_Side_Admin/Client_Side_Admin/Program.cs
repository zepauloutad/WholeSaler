using System;
using System.Threading.Tasks;

namespace Client_Side_Admin
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new Clients.TelecomClient();

            try
            {
                Console.WriteLine("Choose operation:");
                Console.WriteLine("1 - Reserve");
                Console.WriteLine("2 - Activate");
                Console.WriteLine("3 - Deactivate");
                Console.WriteLine("4 - Terminate");

                var choice = Console.ReadLine();

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
    }
}
