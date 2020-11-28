using Domain.Entities;
using Domain.Enums;
using WebApp.ViewModels.Common;
using WebApp.ViewModels.UserViewModels;

namespace WebApp.ViewModels.AccessRequestViewModels
{
    public class AccessRequestViewModel : BaseViewModel
    {
        public AccessRequestViewModel(AccessRequest accessRequest, UserViewModel userViewModel) : base(accessRequest)
        {
            AccessType = accessRequest.AccessType;
            ArticleId = accessRequest.ArticleId;
            UserViewModel = userViewModel;
        }

        public AccessType AccessType { get; set; }
        public int ArticleId { get; set; }
        public UserViewModel UserViewModel { get; set; }

    }
}
