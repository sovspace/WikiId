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
    public class GetSubarticleById : IRequest<Subarticle>
    {
        public IEnumerable<string> UserRoles { get; set; }
        public int SubarticleId { get; set; }
        public class GetSubarticleByIdHandler : BaseHandler, IRequestHandler<GetSubarticleById, Subarticle>
        {
            public GetSubarticleByIdHandler(IApplicationDbContext context) : base(context)
            {
            }

            public async Task<Subarticle> Handle(GetSubarticleById request, CancellationToken cancellationToken)
            {
                Subarticle subarticle = await _context.Subarticles.Where(s => s.Id == request.SubarticleId).SingleOrDefaultAsync();

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
