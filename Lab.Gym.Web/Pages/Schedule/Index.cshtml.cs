using AutoMapper;
using Lab.Gym.Web.Application.Features.Events.Commands;
using Lab.Gym.Web.Application.Features.Events.Queries;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public void OnGet()
        {

        }

        [HttpGet]
        public async Task<IActionResult> GetCalendarEvents(string start, string end)
        {
            var events = await _mediator.Send(new GetByDateRequest()
            {
                Start = DateTime.SpecifyKind(DateTime.Parse(start), DateTimeKind.Utc),
                End = DateTime.SpecifyKind(DateTime.Parse(end), DateTimeKind.Utc),
            });

            return new JsonResult(_mapper.Map<List<EventVm>>(events));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEvent([FromBody] EventVm evt)
        {
            string message = String.Empty;

            await _mediator.Send(_mapper.Map<UpdateRequest>(evt));

            return new JsonResult(new { message });
        }

        [HttpPost]
        public async Task<JsonResult> AddEvent([FromBody] EventVm newEvent)
        {
            string message = String.Empty;
            var createRequest = _mapper.Map<CreateRequest>(newEvent);

            createRequest.Id = Guid.NewGuid();
            await _mediator.Send(createRequest);

            return new JsonResult(new { message, createRequest.Id });
        }

        [HttpPost]
        public async Task<JsonResult> DeleteEvent([FromBody] DeleteEventRequest request)
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
