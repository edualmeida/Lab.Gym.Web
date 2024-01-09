using Lab.Gym.Web.Application.Models;

namespace Lab.Gym.Web.Application.Exceptions
{
    public class RequestException: Exception
    {
        public IList<RequestErrorDetail> Errors { get; set; } = new List<RequestErrorDetail>();

        public RequestException(IList<RequestErrorDetail> errors) 
        {
            Errors = errors;
        }
    }
}
