using AutoMapper;
using Lab.Gym.Web.Application.Configuration;
using Lab.Gym.Web.Application.Features.ScheduleEvents.Commands;
using Lab.Gym.Web.Application.Features.ScheduleEvents.Queries;
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
            List<ScheduleEventModel> events = await _mediator.Send(new GetByDateRequest()
            {
                Start = DateTime.SpecifyKind(DateTime.Parse(start), DateTimeKind.Utc),
                End = DateTime.SpecifyKind(DateTime.Parse(end), DateTimeKind.Utc),
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
            //var createRequest = _mapper.Map<CreateRequest>(newEvent);
            string createdId = "";
            try
            {
                string st = scheduleEvent.Start;
                _logger.LogWarning("OnPostEvent->Start: '" + st + "'.");
                _logger.LogWarning("OnPostEvent->End: '" + scheduleEvent.End + "'.");

                DateTime start = new DateTime(int.Parse(st.Substring(6,4)), int.Parse(st.Substring(3, 2)), int.Parse(st.Substring(0, 2)));//DateTime.ParseExact(scheduleEvent.Start, "dd/MM/yyyy h:mm t", CultureInfo.InvariantCulture);
                DateTime? end = DateTime.Now.AddMinutes(30);// string.IsNullOrEmpty(scheduleEvent.End) ? null : DateTime.ParseExact(scheduleEvent.End, "dd/MM/yyyy h:mm t", CultureInfo.InvariantCulture);
                
                var createRequest = new CreateRequest()
                {
                    AllDay = scheduleEvent.AllDay,
                    Description = scheduleEvent.Description,
                    End = end,
                    Start = start,
                    Title = scheduleEvent.Title,
                    Id = Guid.NewGuid()
                };

                await _mediator.Send(createRequest);
                createdId = createRequest.Id.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogWarning("2OnPostEvent->Start: '" + scheduleEvent.Start + "'.");
                _logger.LogWarning("2OnPostEvent->End: '" + scheduleEvent.End + "'.");
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
