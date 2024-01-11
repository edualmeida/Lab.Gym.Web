using Lab.Gym.Web.Domain.Models;
using MediatR;

namespace Lab.Gym.Web.Application.Features.Events.Queries
{
    public class GetByDateRequest : IRequest<List<ScheduleEvent>>
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
