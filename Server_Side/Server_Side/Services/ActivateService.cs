namespace Server_Side.Services
{
    public class ActivateService
    {
        public ActivateResponse Activate(string adminNumber)
        {
            var response = new ActivateResponse();

            response.EstimatedTime = "5 minutes";
            response.ActivationStatus = true;

            return response;
        }
    }

    public class ActivateResponse
    {
        public string EstimatedTime { get; set; }
        public bool ActivationStatus { get; set; }
    }
}
