namespace Lab.Gym.Web.Application.Models
{
    public class HttpCallResult
    {
        public bool Succeeded { get; set; }
        public string ErrorMessage { get; set; } = "";

        public HttpCallResult(string errorMessage)
        {
            Succeeded = false;
            ErrorMessage = errorMessage;
        }

        public HttpCallResult(bool succeeded)
        {  
            Succeeded = succeeded;
        }
    }
}
