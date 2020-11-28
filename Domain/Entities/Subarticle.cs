using Domain.Common;

namespace Domain.Entities
{
    public class Subarticle : DatedEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
    }
}
