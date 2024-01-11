using Lab.Gym.Web.Repository.Models;

namespace Lab.Gym.Web.Repository
{
    public interface IScheduleEventRepository
    {
        public Task<List<ScheduleEventRepo>> GetByDateRange(DateTime start, DateTime? end);
        public Task CreateAsync(ScheduleEventRepo schedule);
        public Task UpdateAsync(ScheduleEventRepo schedule);
        public Task DeleteAsync(Guid Id);
    }
}
