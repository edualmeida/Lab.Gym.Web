using AutoMapper;
using Lab.Gym.Web.Application.Configuration;
using Lab.Gym.Web.Application.Features.ScheduleEvents.Commands;
using Lab.Gym.Web.Application.Features.ScheduleEvents.Queries;
using Lab.Gym.Web.Constants;
using Lab.Gym.Web.Domain.Models;
using Lab.Gym.Web.Pages.Shared.Components;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Lab.Gym.Web.Pages.Schedule
{
    public class IndexModel : UserPageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public bool IsManager { get; set; }

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
            _logger.LogDebug("OnGetCalendarEvents->start: '" + start);
            _logger.LogDebug("OnGetCalendarEvents->end: '" + end);
            List<ScheduleEventModel> events = await _mediator.Send(new GetByDateRequest()
            {
                Start = DateTime.ParseExact(start, FormatConstants.SourceDateFormat, new CultureInfo("en-IE")),
                End = DateTime.ParseExact(end, FormatConstants.SourceDateFormat, new CultureInfo("en-IE")),
            });

            var mappedEvents = _mapper.Map<List<ScheduleEventVm>>(events);
            return new JsonResult(mappedEvents);
        }

        public async Task<JsonResult> OnPostUpdateEvent([FromBody] ScheduleEventVm scheduleEvent)
        {
            Authorize();

            string message = String.Empty;

            await _mediator.Send(_mapper.Map<UpdateRequest>(scheduleEvent));

            return new JsonResult(new { message });
        }

        public async Task<JsonResult> OnPostEvent([FromBody] ScheduleEventVm scheduleEvent)
        {
            Authorize();

            string message = String.Empty;
            string createdId;
            try
            {
                _logger.LogDebug("OnPostEvent->Start: '" + scheduleEvent.Start);
                _logger.LogDebug("OnPostEvent->End: '" + scheduleEvent.End);

                await _mediator.Send(_mapper.Map<CreateRequest>(scheduleEvent));
                createdId = scheduleEvent.Id.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when creating schedule event.");
                throw;
            }

            return new JsonResult(new { message, createdId });
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
