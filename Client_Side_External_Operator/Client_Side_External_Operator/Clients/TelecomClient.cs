using System;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Server_Side;

namespace Client_Side_External_Operator.Clients
{
    public class TelecomClient
    {
        GrpcChannel channel;
        Telecom.TelecomClient client;

        public TelecomClient()
        {
            var httpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler());
            channel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions { HttpHandler = httpHandler });
            client = new Telecom.TelecomClient(channel);
        }

        public void ReserveDomicile(string domicile, string mode)
        {
            var request = new ReserveRequest { Domicile = domicile, Mode = mode };
            var reply = client.Reserve(request);
            Console.WriteLine($"Admin number: {reply.AdminNumber} - Reservation status: {reply.ReservationStatus}");
        }

        public async Task ActivateAsync(string adminNumber)
        {
            var reply = await client.ActivateAsync(
                              new ActivateRequest { AdminNumber = adminNumber });
            Console.WriteLine($"Estimated time: {reply.EstimatedTime} - Activation status: {reply.ActivationStatus}");
        }

        public async Task DeactivateAsync(string adminNumber)
        {
            var reply = await client.DeactivateAsync(
                              new DeactivateRequest { AdminNumber = adminNumber });
            Console.WriteLine($"Estimated time: {reply.EstimatedTime} - Deactivation status: {reply.DeactivationStatus}");
        }

        public async Task TerminateAsync(string adminNumber)
        {
            var reply = await client.TerminateAsync(
                              new TerminateRequest { AdminNumber = adminNumber });
            Console.WriteLine($"Estimated time: {reply.EstimatedTime} - Termination status: {reply.TerminationStatus}");
        }

        public void Shutdown()
        {
            channel.ShutdownAsync().Wait();
        }
    }
}
