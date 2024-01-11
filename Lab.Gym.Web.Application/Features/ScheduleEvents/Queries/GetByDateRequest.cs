using Lab.Gym.Web.Domain.Models;
using MediatR;

namespace Lab.Gym.Web.Application.Features.ScheduleEvents.Queries
{
    public class GetByDateRequest : IRequest<List<ScheduleEventModel>>
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
