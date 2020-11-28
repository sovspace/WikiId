using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class AccessRequest : BaseEntity
    {
        public Profile Profile { get; set; }
        public int ProfileId { get; set; }
        public Article Article { get; set; }
        public int ArticleId { get; set; }
        public AccessType AccessType { get; set; }
    }
}
