namespace Lab.Gym.Web.Pages.Schedule
{
    public class EventVm
    {
        public string Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string Start { get; set; }
        public string? End { get; set; }
        public bool AllDay { get; set; }

        public EventVm()
        {
            Id = "";
        }
    }
}
