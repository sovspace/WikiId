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
    public class GetPublicArticlesQuery : IRequest<IEnumerable<Article>>
    {
        public class GetPublicArticlesQueryHandler : BaseHandler, IRequestHandler<GetPublicArticlesQuery, IEnumerable<Article>>
        {
            public GetPublicArticlesQueryHandler(IApplicationDbContext context) : base(context)
            {
            }

            public async Task<IEnumerable<Article>> Handle(GetPublicArticlesQuery request, CancellationToken cancellationToken)
            {
                return await _context.Articles
                    .Include(a => a.TitleImage)
                    .Include(a => a.Category)
                    .Include(a => a.Creator)
                    .Where(a => a.IsPublic)
                    .ToListAsync();
            }
        }
    }
}
