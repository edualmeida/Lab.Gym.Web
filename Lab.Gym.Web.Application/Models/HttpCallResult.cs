namespace Lab.Gym.Web.Application.Models
{
    public class HttpCallResult
    {
        public bool Succeeded { get; set; }
        public IList<RequestErrorDetail> Errors { get; set; } = new List<RequestErrorDetail>();

        public HttpCallResult(IList<RequestErrorDetail> errors)
        {
            Succeeded = false;
            Errors = errors;
        }

        public HttpCallResult(bool succeeded)
        {
            Succeeded = succeeded;
        }
    }
}
