using Domain.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApp.ViewModels.AccessRequestViewModels;
using WebApp.ViewModels.Common;
using WebApp.ViewModels.UserViewModels;

namespace WebApp.ViewModels.ArticleViewModels
{
    public class CreatedArticleViewModel : DatedViewModel
    {
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Title image")]
        public string TitleImagePath { get; set; }

        [Display(Name = "Is public")]
        public bool IsPublic { get; set; }

        [Display(Name = "Category name")]
        public string CategoryTitle { get; set; }
        public int? CategoryId { get; set; }

        public UserViewModel Creator { get; set; }
        public IEnumerable<AccessRequestViewModel> AccessRequestViewModels { get; set; }
        public IEnumerable<UserViewModel> CanEditUsers { get; set; }
        public IEnumerable<UserViewModel> CanViewUsers { get; set; }
        public CreatedArticleViewModel(Article article,
            IEnumerable<AccessRequestViewModel> accessRequestViewModels,
            IEnumerable<UserViewModel> canEditUsers,
            IEnumerable<UserViewModel> canViewUsers,
            UserViewModel userViewModel) : base(article)
        {
            Title = article.Title;
            TitleImagePath = article?.TitleImage?.Path;
            IsPublic = article.IsPublic;
            CategoryTitle = article?.Category.Title;
            CategoryId = article?.Category.Id;
            Creator = userViewModel;
            CanEditUsers = canEditUsers;
            CanViewUsers = canViewUsers;
            AccessRequestViewModels = accessRequestViewModels;
        }
    }
}
