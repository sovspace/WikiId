using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using WebApp.ViewModels.Common;

namespace WebApp.ViewModels.ArticleViewModels
{
    public class ArticleViewModel : DatedViewModel
    {

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Title image")]
        public string TitleImagePath { get; set; }

        [Display(Name = "Creator username")]
        public string UserName { get; set; }

        [Display(Name = "Is public")]
        public bool IsPublic { get; set; }

        [Display(Name = "Category name")]
        public string CategoryTitle { get; set; }
        public int? CategoryId { get; set; }

        public ArticleViewModel(Article article, string creatorUsername) : base(article)
        {
            Title = article.Title;
            TitleImagePath = article?.TitleImage?.Path;
            IsPublic = article.IsPublic;
            CategoryTitle = article?.Category.Title;
            CategoryId = article?.Category.Id;

            UserName = creatorUsername;
        }
    }
}
