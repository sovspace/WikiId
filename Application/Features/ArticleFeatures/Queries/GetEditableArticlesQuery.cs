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
    public class GetEditableArticlesQuery : IRequest<IEnumerable<Article>>
    {
        public IEnumerable<string> UserRoles { get; set; }

        public class GetEditableArticlesQueryHandle : BaseHandler, IRequestHandler<GetEditableArticlesQuery, IEnumerable<Article>>
        {
            public GetEditableArticlesQueryHandle(IApplicationDbContext context) : base(context)
            {
            }

            public async Task<IEnumerable<Article>> Handle(GetEditableArticlesQuery request, CancellationToken cancellationToken)
            {

                return await _context.Articles
                    .Where(a => request.UserRoles.Contains(a.EditRoleString))
                    .Include(a => a.TitleImage)
                    .Include(a => a.Category)
                    .Include(a => a.Creator)
                    .ToListAsync();
            }
        }

    }
}
