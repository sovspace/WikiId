using Microsoft.AspNetCore.Identity;

namespace WebApp.ViewModels.UserViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        public UserViewModel(IdentityUser user)
        {
            Id = user.Id;
            UserName = user.UserName;
        }
    }
}
