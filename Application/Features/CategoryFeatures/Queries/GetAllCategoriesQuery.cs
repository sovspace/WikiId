using Application.Features.Common;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.CategoryFeatures.Queries
{
    public class GetAllCategoriesQuery : IRequest<IEnumerable<Category>>
    {
        public class GetAllCategoriesQueryHandler : BaseHandler, IRequestHandler<GetAllCategoriesQuery, IEnumerable<Category>>
        {
            public GetAllCategoriesQueryHandler(IApplicationDbContext context) : base(context)
            {
            }

            public async Task<IEnumerable<Category>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
            {
                return await _context.Categories
                    .Include(c => c.TitleImage)
                    .Include(c => c.Creator)
                    .ToListAsync();
            }
        }
    }
}
