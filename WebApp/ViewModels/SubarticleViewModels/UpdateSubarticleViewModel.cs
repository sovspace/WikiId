using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels.SubarticleViewModels
{
    public class UpdateSubarticleViewModel
    {
        public int SubarticleId { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Content")]
        public string Content { get; set; }
    }
}
