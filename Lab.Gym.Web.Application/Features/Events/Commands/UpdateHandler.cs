using AutoMapper;
using Lab.Gym.Web.Repository;
using Lab.Gym.Web.Repository.Models;
using MediatR;

namespace Lab.Gym.Web.Application.Features.Events.Commands
{
    public class UpdateHandler(
        IScheduleEventRepository _eventsRepository,
        IMapper _mapper) : IRequestHandler<UpdateRequest>
    {
        public Task Handle(UpdateRequest request, CancellationToken cancellationToken)
        {
            return _eventsRepository.UpdateAsync(_mapper.Map<ScheduleEventRepo>(request));
        }
    }
}
