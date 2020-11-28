using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels.ArticleViewModels
{
    public class CreateArticleViewModel
    {
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Is public")]
        public bool IsPublic { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }


        [Display(Name = "Title image")]
        public IFormFile TitleImage { get; set; }

        public SelectList Categories;
    }
}
