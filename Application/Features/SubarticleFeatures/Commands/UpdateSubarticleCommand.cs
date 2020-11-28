using Application.Features.Common;
using Application.Features.SubarticleFeatures.Responses;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SubarticleFeatures.Commands
{
    public class UpdateSubarticleCommand : IRequest<UpdateSubarticleResponse>
    {
        public IEnumerable<string> UserRoles { get; set; }
        public int SubarticleId { get; set; }
        public string Title { get; set; } = null;
        public string Content { get; set; } = null;

        public class UpdateSubarticleCommandHandler : BaseHandler, IRequestHandler<UpdateSubarticleCommand, UpdateSubarticleResponse>
        {
            public UpdateSubarticleCommandHandler(IApplicationDbContext context) : base(context)
            {
            }

            public async Task<UpdateSubarticleResponse> Handle(UpdateSubarticleCommand request, CancellationToken cancellationToken)
            {
                Subarticle subarticle = await _context.Subarticles.Where(s => s.Id == request.SubarticleId).SingleOrDefaultAsync();

                if (subarticle == null)
                {
                    return new UpdateSubarticleResponse
                    {
                        IsSuccessful = false,
                        Message = "Subarticle not found",
                    };
                }

                if (request.UserRoles.Contains(subarticle.Article.EditRoleString))
                {
                    if (request.Title != null)
                    {
                        subarticle.Title = request.Title;
                    }
                    if (request.Content != null)
                    {
                        subarticle.Content = request.Content;
                    }

                    _context.Subarticles.Update(subarticle);
                    await _context.SaveChangesAsync();
                    return new UpdateSubarticleResponse
                    {
                        IsSuccessful = true,
                        Id = subarticle.Id,
                    };
                }
                else
                {
                    return new UpdateSubarticleResponse
                    {
                        IsSuccessful = false,
                        Message = "Can't update subarticle",
                    };
                }

            }
        }

    }
}
