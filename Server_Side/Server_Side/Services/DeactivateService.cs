namespace Server_Side.Services
{
    public class DeactivateService
    {
        public DeactivateResponse Deactivate(string adminNumber)
        {

            var response = new DeactivateResponse();

            response.EstimatedTime = "5 minutes";
            response.DeactivationStatus = true;

            return response;
        }
    }

    public class DeactivateResponse
    {
        public string EstimatedTime { get; set; }
        public bool DeactivationStatus { get; set; }
    }
}
