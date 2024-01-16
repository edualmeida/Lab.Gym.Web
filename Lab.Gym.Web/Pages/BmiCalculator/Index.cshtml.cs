using Lab.Gym.Web.Pages.Shared.Components;
using Microsoft.AspNetCore.Mvc;

namespace Lab.Gym.Web.Pages.BmiCalculator
{
    public class IndexModel(ILogger<IndexModel> logger) : UserPageModel
    {
        [TempData]
        public string StatusMessage { get; set; }

        public void OnGet()
        {
        }
    }
}
