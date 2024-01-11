
using System.ComponentModel.DataAnnotations;

namespace Lab.Gym.Web.Repository.Models
{
    public class ScheduleEventRepo
    {
        [Required]
        public Guid Id { get; set; }
        [StringLength(300)]
        public string? Title { get; set; }
        [StringLength(3000)]
        public string? Description { get; set; }
        [Required]
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        [Required]
        public bool AllDay { get; set; }
    }
}
