using Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels.Common
{
    public class DatedViewModel : BaseViewModel
    {
        public DatedViewModel(DatedEntity datedEntity) : base(datedEntity)
        {
            CreatedDate = datedEntity.CreatedDate;
            UpdatedDate = datedEntity.UpdatedDate;
        }

        [Display(Name = "Creation date")]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Last updated date")]
        [DataType(DataType.Date)]
        public DateTime UpdatedDate { get; set; }



    }
}
