using Application.Features.Common;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ArticleFeatures.Queries
{
    public class GetCreatedArticlesQuery : IRequest<IEnumerable<Article>>
    {
        public string IdentityUserId { get; set; }

        public class GetCrearedArticleQueryHandler : BaseHandler, IRequestHandler<GetCreatedArticlesQuery, IEnumerable<Article>>
        {
            public GetCrearedArticleQueryHandler(IApplicationDbContext context) : base(context)
            {
            }

            public async Task<IEnumerable<Article>> Handle(GetCreatedArticlesQuery request, CancellationToken cancellationToken)
            {
                return await _context.Articles
                    .Include(a => a.Creator)
                    .Where(a => a.Creator.IdentityUserId == request.IdentityUserId)
                    .Include(a => a.TitleImage)
                    .Include(a => a.Category)
                    .Include(a => a.AccessRequests)
                    .ThenInclude(ar => ar.Profile)
                    .ToListAsync();
            }
        }

    }
}
