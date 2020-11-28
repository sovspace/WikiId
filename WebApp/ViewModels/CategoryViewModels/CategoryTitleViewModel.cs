using Domain.Common;
using WebApp.ViewModels.Common;

namespace WebApp.ViewModels.CategoryViewModels
{
    public class CategoryTitleViewModel : BaseViewModel
    {
        public CategoryTitleViewModel(BaseEntity baseEntity, string title) : base(baseEntity)
        {
            Title = title;
        }

        public string Title { get; set; }

    }
}
