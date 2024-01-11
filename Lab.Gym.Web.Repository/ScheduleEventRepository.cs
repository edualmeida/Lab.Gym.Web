using AutoMapper;
using Lab.Gym.Web.Repository.Data.Contexts;
using Lab.Gym.Web.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Lab.Gym.Web.Repository
{
    public class ScheduleEventRepository(
        ScheduleContext _context,
        IMapper _mapper) : IScheduleEventRepository
    {
        public async Task CreateAsync(ScheduleEventRepo schedule)
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

        public async Task UpdateAsync(ScheduleEventRepo scheduleEvent)
        {
            ScheduleEventRepo? foundEvent = FindEvent(scheduleEvent.Id);

            foundEvent = _mapper.Map<ScheduleEventRepo>(scheduleEvent);

            _context.ScheduleEvents.Update(foundEvent);

            await _context.SaveChangesAsync();
        }

        public Task<List<ScheduleEventRepo>> GetByDateRange(DateTime start, DateTime? end)
        {
            var query =
                from ev in _context.ScheduleEvents
                where ev.Start >= start && (ev.End.HasValue && ev.End <= end)
                select ev;

            return query.ToListAsync();
        }

        private ScheduleEventRepo FindEvent(Guid eventId)
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
