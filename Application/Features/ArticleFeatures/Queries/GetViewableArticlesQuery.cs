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
    public class GetViewableArticlesQuery : IRequest<IEnumerable<Article>>
    {
        public IEnumerable<string> UserRoles { get; set; }


        public class GetViewableArticlesQueryHandler : BaseHandler, IRequestHandler<GetViewableArticlesQuery, IEnumerable<Article>>
        {
            public GetViewableArticlesQueryHandler(IApplicationDbContext context) : base(context)
            {
            }

            public async Task<IEnumerable<Article>> Handle(GetViewableArticlesQuery request, CancellationToken cancellationToken)
            {
                return await _context.Articles
                    .Where(a => request.UserRoles.Contains(a.ViewRoleString) || a.IsPublic)
                    .Include(a => a.TitleImage)
                    .Include(a => a.Category)
                    .Include(a => a.Creator)
                    .ToListAsync();
            }
        }

    }
}
