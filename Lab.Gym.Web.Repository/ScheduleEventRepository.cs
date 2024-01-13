using AutoMapper;
using Lab.Gym.Web.Repository.Data.Contexts;
using Lab.Gym.Web.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Lab.Gym.Web.Repository
{
    public class ScheduleEventRepository(
        GymWebDbContext _context,
        IMapper _mapper) : IScheduleEventRepository
    {
        public async Task CreateAsync(ScheduleEvent schedule)
        {
            await _context.AddAsync(schedule);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid eventId)
        {
            var foundEvent = FindEvent(eventId);

            _context.ScheduleEvents.Remove(foundEvent);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ScheduleEvent scheduleEvent)
        {
            ScheduleEvent? foundEvent = FindEvent(scheduleEvent.Id);

            if(null == foundEvent)
            {
                throw new Exception($"ScheduleEvent not found: {scheduleEvent.Id}");
            }

            foundEvent.Description = scheduleEvent.Description;
            foundEvent.End = scheduleEvent.End;
            foundEvent.Start = scheduleEvent.Start;
            foundEvent.AllDay = scheduleEvent.AllDay;
            foundEvent.Title = scheduleEvent.Title;

            //_context.ScheduleEvents.Update(foundEvent);

            await _context.SaveChangesAsync();
        }

        public Task<List<ScheduleEvent>> GetByDateRange(DateTime start, DateTime? end)
        {
            var query =
                from ev in _context.ScheduleEvents
                where ev.Start >= start && (ev.End.HasValue && ev.End <= end)
                select ev;

            return query.ToListAsync();
        }

        private ScheduleEvent FindEvent(Guid eventId)
        {
            var foundEvent = _context.ScheduleEvents.FirstOrDefault(x => x.Id == eventId);

            if (foundEvent == null)
            {
                throw new Exception("ScheduleEvent not found");
            }

            return foundEvent;
        }

    }
}
