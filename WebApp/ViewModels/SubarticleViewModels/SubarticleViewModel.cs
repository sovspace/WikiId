using Domain.Entities;

using System.ComponentModel.DataAnnotations;
using WebApp.ViewModels.Common;

namespace WebApp.ViewModels.SubarticleViewModels
{
    public class SubarticleViewModel : DatedViewModel
    {
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Content")]
        public string Content { get; set; }

        public SubarticleViewModel(Subarticle s) : base(s)
        {
            Title = s.Title;
            Content = s.Content;
        }
    }
}
