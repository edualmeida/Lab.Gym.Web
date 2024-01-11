using Lab.Gym.Web.Domain.Models;
using MediatR;

namespace Lab.Gym.Web.Application.Features.ScheduleEvents.Commands
{
    public class UpdateRequest : ScheduleEventModel, IRequest
    {
    }
}
