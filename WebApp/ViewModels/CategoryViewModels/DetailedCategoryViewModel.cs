using Domain.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApp.ViewModels.ArticleViewModels;
using WebApp.ViewModels.Common;

namespace WebApp.ViewModels.CategoryViewModels
{
    public class DetailedCategoryViewModel : DatedViewModel
    {
        [Display(Name = "Category title")]
        public string Title { get; set; }

        [Display(Name = "Category title image")]
        public string TitleImagePath { get; set; }

        [Display(Name = "Category creator")]
        public string CreatorUserName { get; set; }
        public IEnumerable<ArticleViewModel> Articles { get; set; }

        public DetailedCategoryViewModel(Category c, string userName, IEnumerable<ArticleViewModel> articlesViewModel) : base(c)
        {
            Title = c.Title;
            TitleImagePath = c?.TitleImage?.Path;
            CreatorUserName = userName;
            Articles = articlesViewModel;
        }
    }
}
