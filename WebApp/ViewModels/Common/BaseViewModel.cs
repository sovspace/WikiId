using Domain.Common;

namespace WebApp.ViewModels.Common
{
    public class BaseViewModel
    {
        public int Id { get; set; }

        public BaseViewModel(BaseEntity baseEntity)
        {
            Id = baseEntity.Id;
        }
    }
}
