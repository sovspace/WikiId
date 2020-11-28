using Application.Features.Common;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ArticleFeatures.Queries
{
    public class GetArticleByIdQuery : IRequest<Article>
    {
        public int Id { get; set; }
        public IEnumerable<string> UserRoles { get; set; }
        public class GetArticleByIdQueryHandler : BaseHandler, IRequestHandler<GetArticleByIdQuery, Article>
        {
            public GetArticleByIdQueryHandler(IApplicationDbContext context) : base(context)
            {
            }

            public async Task<Article> Handle(GetArticleByIdQuery request, CancellationToken cancellationToken)
            {
                Article article = await _context.Articles
                    .Include(a => a.Category)
                    .Include(a => a.TitleImage)
                    .Include(a => a.Gallery)
                    .Include(a => a.Subarticles)
                    .Include(a => a.Creator)
                    .Where(a => a.Id == request.Id && (request.UserRoles.Contains(a.ViewRoleString) || a.IsPublic)).SingleOrDefaultAsync();
                return article;
            }
        }

    }
}
