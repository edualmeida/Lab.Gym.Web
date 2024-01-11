using Lab.Gym.Web.Domain.Models;
using MediatR;

namespace Lab.Gym.Web.Application.Features.Events.Commands
{
    public class CreateRequest : ScheduleEvent, IRequest
    {
    }
}
