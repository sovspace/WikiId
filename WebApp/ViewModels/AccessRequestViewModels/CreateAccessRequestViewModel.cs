using Domain.Enums;

namespace WebApp.ViewModels.AccessRequestViewModels
{
    public class CreateAccessRequestViewModel
    {
        public int ArticleId { get; set; }
        public AccessType AccessType { get; set; }
    }
}
