//using test.Sessions.Dto;

namespace Lab.Gym.Web.Pages.Shared.Components.RightNavbarUserArea
{
    public class RightNavbarUserAreaViewModel
    {
        //public GetCurrentLoginInformationsOutput LoginInformations { get; set; }
        public string Username { get; set; } = "-";
        public bool IsAuthenticated { get; set; } = false;

        public string GetShownLoginName()
        {
            var userName = Username;//LoginInformations.User.UserName;

            return userName;
            //return LoginInformations.Tenant == null
            //    ? ".\\" + userName
            //    : LoginInformations.Tenant.TenancyName + "\\" + userName;
        }
    }
}

