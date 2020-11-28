using Domain.Common;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Article : DatedEntity
    {
        public string Title { get; set; }
        public MediaFile TitleImage { get; set; }
        public bool IsPublic { get; set; }
        public string EditRoleString { get; set; }
        public string ViewRoleString { get; set; }
        public Profile Creator { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<AccessRequest> AccessRequests { get; set; }
        public List<Subarticle> Subarticles { get; set; }
        public List<MediaFile> Gallery { get; set; }
    }
}
