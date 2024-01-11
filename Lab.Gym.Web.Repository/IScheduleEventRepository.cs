using Lab.Gym.Web.Repository.Models;

namespace Lab.Gym.Web.Repository
{
    public interface IScheduleEventRepository
    {
        public Task<List<ScheduleEvent>> GetByDateRange(DateTime start, DateTime? end);
        public Task CreateAsync(ScheduleEvent schedule);
        public Task UpdateAsync(ScheduleEvent schedule);
        public Task DeleteAsync(Guid Id);
    }
}
