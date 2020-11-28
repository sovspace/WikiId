using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels.SubarticleViewModels
{
    public class CreateSubarticleViewModel
    {
        public int ArticleId { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Content")]
        public string Content { get; set; }
    }
}
