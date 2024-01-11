using AutoMapper;
using Lab.Gym.Web.Repository;
using Lab.Gym.Web.Repository.Models;
using MediatR;

namespace Lab.Gym.Web.Application.Features.ScheduleEvents.Commands
{
    public class CreateHandler(
        IScheduleEventRepository _eventsRepository,
        IMapper _mapper) : IRequestHandler<CreateRequest>
    {
        public Task Handle(CreateRequest request, CancellationToken cancellationToken)
        {
            return _eventsRepository.CreateAsync(_mapper.Map<ScheduleEvent>(request));
        }
    }
}
