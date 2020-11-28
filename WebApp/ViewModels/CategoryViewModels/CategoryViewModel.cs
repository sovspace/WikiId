using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using WebApp.ViewModels.Common;

namespace WebApp.ViewModels.CategoryViewModels
{
    public class CategoryViewModel : DatedViewModel
    {
        [Display(Name = "Category title")]
        public string Title { get; set; }

        [Display(Name = "Category title image")]
        public string TitleImagePath { get; set; }

        [Display(Name = "Category creator")]
        public string CreatorUserName { get; set; }

        public CategoryViewModel(Category c, string userName) : base(c)
        {
            Title = c.Title;
            TitleImagePath = c?.TitleImage?.Path;
            CreatorUserName = userName;
        }
    }
}
