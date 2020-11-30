using Application.Features.Common;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SubarticleFeatures.Queries
{
    public class GetSubarticleByIdQuery : IRequest<Subarticle>
    {
        public IEnumerable<string> UserRoles { get; set; }
        public int SubarticleId { get; set; }
        public class GetSubarticleByIdQueryHandler : BaseHandler, IRequestHandler<GetSubarticleByIdQuery, Subarticle>
        {
            public GetSubarticleByIdQueryHandler(IApplicationDbContext context) : base(context)
            {
            }

            public async Task<Subarticle> Handle(GetSubarticleByIdQuery request, CancellationToken cancellationToken)
            {
                Subarticle subarticle = await _context.Subarticles
                    .Where(s => s.Id == request.SubarticleId)
                    .Include(s => s.Article)
                    .SingleOrDefaultAsync();

                if (subarticle == null)
                {
                    return null;
                }
                else
                {
                    if (request.UserRoles.Contains(subarticle.Article.ViewRoleString))
                    {
                        return subarticle;
                    }
                    else
                    {
                        return null;
                    }
                }

            }
        }
    }
}
