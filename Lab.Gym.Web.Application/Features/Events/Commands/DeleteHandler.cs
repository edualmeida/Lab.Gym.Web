using AutoMapper;
using Lab.Gym.Web.Repository;
using MediatR;

namespace Lab.Gym.Web.Application.Features.Events.Commands
{
    public class DeleteHandler(
        IScheduleEventRepository _eventsRepository,
        IMapper _mapper) : IRequestHandler<DeleteRequest>
    {
        public Task Handle(DeleteRequest request, CancellationToken cancellationToken)
        {
            return _eventsRepository.DeleteAsync(request.Id);
        }
    }
}
