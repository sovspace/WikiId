using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels.CategoryViewModels
{
    public class CreateCategoryViewModel
    {
        [Display(Name = "Category title")]
        public string Title { get; set; }

        [Display(Name = "Title image")]
        public IFormFile TitleImage { get; set; }

    }
}
