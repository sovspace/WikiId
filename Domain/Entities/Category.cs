using Domain.Common;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Category : DatedEntity
    {
        public MediaFile TitleImage { get; set; }
        public string Title { get; set; }
        public Profile Creator { get; set; }
        public List<Article> Articles { get; set; }

    }
}
