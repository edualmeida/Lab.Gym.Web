using MediatR;

namespace Lab.Gym.Web.Application.Features.Events.Commands
{
    public class DeleteRequest : IRequest
    {
        public Guid Id { get; set; }
    }
}
