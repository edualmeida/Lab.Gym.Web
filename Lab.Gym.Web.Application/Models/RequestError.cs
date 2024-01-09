
namespace Lab.Gym.Web.Application.Models
{
    public class RequestError
    {
        public IList<RequestErrorDetail> Errors { get; set; } = new List<RequestErrorDetail>();
    }
}
