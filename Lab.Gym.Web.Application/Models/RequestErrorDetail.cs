
namespace Lab.Gym.Web.Application.Models
{
    public class RequestErrorDetail
    {
        public string Code { get; set; } = "";
        public string Description { get; set; } = "";

        public RequestErrorDetail(string code, string description)
        {
            Code = code;
            Description = description;
        }
    }
}
