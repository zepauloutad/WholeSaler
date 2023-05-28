namespace Server_Side.Services
{
    public class TerminateService
    {
        public TerminateResponse Terminate(string adminNumber)
        {

            var response = new TerminateResponse();

            response.EstimatedTime = "5 minutes";
            response.TerminationStatus = true;

            return response;
        }
    }

    public class TerminateResponse
    {
        public string EstimatedTime { get; set; }
        public bool TerminationStatus { get; set; }
    }
}