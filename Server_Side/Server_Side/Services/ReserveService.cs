namespace Server_Side.Services
{
    public class ReserveService
    {
        public ReserveResponse Reserve(string domicile, string mode)
        {
            var response = new ReserveResponse();

            response.AdminNumber = "123";  
            response.ReservationStatus = true;

            return response;
        }
    }

    public class ReserveResponse
    {
        public string AdminNumber { get; set; }
        public bool ReservationStatus { get; set; }
    }
}
