using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.CategoryFeatures.Queries
{
    public class GetCategoryByIdQuery : IRequest<Category>
    {
        public int CategoryId { get; set; }
        public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Category>
        {
            private readonly IApplicationDbContext _context;

            public GetCategoryByIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Category> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
            {
                return await _context.Categories.Where(c => c.Id == request.CategoryId)
                    .Include(c => c.Creator)
                    .Include(c => c.Articles)
                    .ThenInclude(a => a.TitleImage)
                    .SingleOrDefaultAsync();
            }
        }
    }
}
