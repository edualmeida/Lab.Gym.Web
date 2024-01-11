using AutoMapper;
using Lab.Gym.Web.Domain.Models;
using Lab.Gym.Web.Repository;
using Lab.Gym.Web.Repository.Models;
using MediatR;

namespace Lab.Gym.Web.Application.Features.Events.Queries
{
    public class GetByDateHandler(
        IScheduleEventRepository _eventsClient,
        IMapper _mapper) : IRequestHandler<GetByDateRequest, List<ScheduleEvent>>
    {
        public async Task<List<ScheduleEvent>> Handle(GetByDateRequest request, CancellationToken cancellationToken)
        {
            var repoEvents = await _eventsClient.GetByDateRange(request.Start, request.End);

            return _mapper.Map<List<ScheduleEventRepo>, List<ScheduleEvent>>(repoEvents);
        }
    }
}
