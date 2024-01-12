using AutoMapper;
using Lab.Gym.Web.Application.Features.ScheduleEvents.Commands;
using Lab.Gym.Web.Application.Features.ScheduleEvents.Queries;
using Lab.Gym.Web.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Lab.Gym.Web.Pages.Schedule
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

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

        }
        public JsonResult OnGetList()
        {
            List<string> lstString = new List<string>
            {
                "Val 1",
                "Val 2",
                "Val 3"
            };
            return new JsonResult(lstString);
        }

        public ActionResult OnPostSend()
        {
            string sPostValue1 = "";
            string sPostValue2 = "";
            string sPostValue3 = "";

            List<string> lstString = new List<string>
            {
                sPostValue1,
                sPostValue2,
                sPostValue3
            };
            return new JsonResult(lstString);
        }

        //public JsonResult OnGetCalendarEvents(string start, string end)
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
            string message = String.Empty;

            await _mediator.Send(_mapper.Map<UpdateRequest>(evt));

            return new JsonResult(new { message });
        }

        public async Task<JsonResult> OnPostEvent([FromBody] ScheduleEventVm newEvent)
        {
            newEvent.Id = Guid.NewGuid().ToString();
            string message = String.Empty;
            var createRequest = _mapper.Map<CreateRequest>(newEvent);

            createRequest.Id = Guid.NewGuid();
            await _mediator.Send(createRequest);

            return new JsonResult(new { message, createRequest.Id });
        }

        public async Task<JsonResult> OnPostDeleteEvent([FromBody] DeleteEventRequest request)
        {
            string message = String.Empty;

            await _mediator.Send(new DeleteRequest()
            {
                Id = request.EventId
            });

            return new JsonResult(new { message });
        }
    }
}
