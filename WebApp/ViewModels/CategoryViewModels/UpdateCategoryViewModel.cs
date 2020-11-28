using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels.CategoryViewModels
{
    public class UpdateCategoryViewModel
    {
        public int CategoryId { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Title image")]
        public IFormFile TitleImage { get; set; }

    }
}
