using Domain.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WebApp.ViewModels.Common;
using WebApp.ViewModels.SubarticleViewModels;

namespace WebApp.ViewModels.ArticleViewModels
{
    public class DetailedArticleViewModel : DatedViewModel
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

        public IEnumerable<SubarticleViewModel> Subarticles { get; set; }
        public IEnumerable<string> GalleryPaths { get; set; }

        public DetailedArticleViewModel(Article article, string userName) : base(article)
        {
            Title = article.Title;
            TitleImagePath = article?.TitleImage?.Path;
            Subarticles = article?.Subarticles?.Select(s => new SubarticleViewModel(s));
            GalleryPaths = article?.Gallery?.Select(g => g.Path);
            CategoryTitle = article?.Category.Title;
            CategoryId = article?.Category.Id;
            UserName = userName;
        }
    }
}
