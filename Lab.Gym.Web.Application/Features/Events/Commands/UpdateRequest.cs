using Lab.Gym.Web.Domain.Models;
using MediatR;

namespace Lab.Gym.Web.Application.Features.Events.Commands
{
    public class UpdateRequest : ScheduleEvent, IRequest
    {
    }
}
