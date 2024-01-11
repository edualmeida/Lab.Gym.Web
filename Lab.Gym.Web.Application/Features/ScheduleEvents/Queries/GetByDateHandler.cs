using AutoMapper;
using Lab.Gym.Web.Domain.Models;
using Lab.Gym.Web.Repository;
using Lab.Gym.Web.Repository.Models;
using MediatR;

namespace Lab.Gym.Web.Application.Features.ScheduleEvents.Queries
{
    public class GetByDateHandler(
        IScheduleEventRepository _eventsClient,
        IMapper _mapper) : IRequestHandler<GetByDateRequest, List<ScheduleEventModel>>
    {
        public async Task<List<ScheduleEventModel>> Handle(GetByDateRequest request, CancellationToken cancellationToken)
        {
            var repoEvents = await _eventsClient.GetByDateRange(request.Start, request.End);

            return _mapper.Map<List<ScheduleEvent>, List<ScheduleEventModel>>(repoEvents);
        }
    }
}
