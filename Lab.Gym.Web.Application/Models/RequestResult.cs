
namespace Lab.Gym.Web.Application.Models
{
    public class RequestResult<TResult>
    {
        public bool Succeeded { get; set; }
        public IList<RequestErrorDetail> IdentityErrors { get; set; } = new RequestErrorDetail[0];
        public TResult? Result { get; set; }
    }
}
