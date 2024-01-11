using MediatR;

namespace Lab.Gym.Web.Application.Features.ScheduleEvents.Commands
{
    public class DeleteRequest : IRequest
    {
        public Guid Id { get; set; }
    }
}
