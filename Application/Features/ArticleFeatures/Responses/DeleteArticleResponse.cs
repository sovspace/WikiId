using Application.Features.Common;

namespace Application.Features.ArticleFeatures.Responses
{
    public class DeleteArticleResponse : BaseResponse
    {
        public int Id { get; set; }
        public string EditRole { get; set; }
        public string ViewRole { get; set; }
    }
}
