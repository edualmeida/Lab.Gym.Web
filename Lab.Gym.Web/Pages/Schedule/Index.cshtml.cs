using AutoMapper;
using Lab.Gym.Web.Application.Configuration;
using Lab.Gym.Web.Application.Features.ScheduleEvents.Commands;
using Lab.Gym.Web.Application.Features.ScheduleEvents.Queries;
using Lab.Gym.Web.Domain.Models;
using Lab.Gym.Web.Pages.Shared.Components;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Lab.Gym.Web.Pages.Schedule
{
    public class IndexModel : UserPageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public bool IsManager { get; set; }

        [TempData]
        public string StatusMessage { get; set; }
        public IndexModel(
            ILogger<IndexModel> logger, 
            IMediator mediator,
            IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));
        }

        public void OnGet()
        {
            IsManager = User.IsInRole(AppConstants.ManagerRoleName);
        }

        public async Task<JsonResult> OnGetCalendarEvents(string start, string end)
        {
            List<ScheduleEventModel> events = await _mediator.Send(new GetByDateRequest()
            {
                Start = DateTime.SpecifyKind(DateTime.Parse(start), DateTimeKind.Utc),
                End = DateTime.SpecifyKind(DateTime.Parse(end), DateTimeKind.Utc),
            });

            var mappedEvents = _mapper.Map<List<ScheduleEventVm>>(events);
            return new JsonResult(mappedEvents);
        }

        public async Task<JsonResult> OnPostUpdateEvent([FromBody] ScheduleEventVm evt)
        {
            Authorize();

            string message = String.Empty;

            await _mediator.Send(_mapper.Map<UpdateRequest>(evt));

            return new JsonResult(new { message });
        }

        public async Task<JsonResult> OnPostEvent([FromBody] ScheduleEventVm newEvent)
        {
            Authorize();

            _logger.LogWarning("OnPostEvent->Start: '" + newEvent.Start + "'.");
            _logger.LogWarning("OnPostEvent->End: '" + newEvent.End + "'.");

            string message = String.Empty;
            //var createRequest = _mapper.Map<CreateRequest>(newEvent);

            if (string.IsNullOrEmpty(newEvent.Start))
            {
                _logger.LogWarning("EMPTY->Start: '" + newEvent.Start + "'.");
            }

            DateTime start = DateTime.Now; //DateTime.ParseExact(newEvent.Start, "dd/MM/yyyy h:mm t", CultureInfo.InvariantCulture);
            DateTime? end = DateTime.Now;  //string.IsNullOrEmpty(newEvent.End) ? null : DateTime.ParseExact(newEvent.End, "dd/MM/yyyy h:mm t", CultureInfo.InvariantCulture);

            var createRequest = new CreateRequest()
            {
                AllDay = newEvent.AllDay,
                Description = newEvent.Description,
                End = end,
                Id = new Guid(newEvent.Id),
                Start = start,
                Title = newEvent.Title,
            };

            createRequest.Id = Guid.NewGuid();
            await _mediator.Send(createRequest);

            return new JsonResult(new { message, createRequest.Id });
        }

        public async Task<JsonResult> OnPostDeleteEvent([FromBody] DeleteEventRequest request)
        {
            Authorize();

            string message = String.Empty;

            await _mediator.Send(new DeleteRequest()
            {
                Id = request.EventId
            });

            return new JsonResult(new { message });
        }
    }
}
