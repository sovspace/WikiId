using Application.Features.Common;

namespace Application.Features.ArticleFeatures.Responses
{
    public class CreateArticleResponse : BaseResponse
    {
        public int ArticleId { get; set; }
        public string EditArticleRoleName { get; set; } = null;
        public string ViewArticleRoleName { get; set; } = null;
    }
}
