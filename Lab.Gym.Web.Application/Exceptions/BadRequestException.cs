using Lab.Gym.Web.Application.Models;

namespace Lab.Gym.Web.Application.Exceptions
{
    public class BadRequestException: Exception
    {
        public IList<RequestErrorDetail> Errors { get; set; } = new List<RequestErrorDetail>();

        public BadRequestException(IList<RequestErrorDetail> errors) 
        {
            Errors = errors;
        }
    }
}
