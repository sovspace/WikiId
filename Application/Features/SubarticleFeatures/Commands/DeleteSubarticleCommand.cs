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
    public class DeleteSubarticleCommand : IRequest<DeleteSubarticleResponse>
    {
        public IEnumerable<string> UserRoles { get; set; }
        public int SubarticleId { get; set; }
        public class DeleteSubarticleCommandHandler : BaseHandler, IRequestHandler<DeleteSubarticleCommand, DeleteSubarticleResponse>
        {
            public DeleteSubarticleCommandHandler(IApplicationDbContext context) : base(context)
            {
            }

            public async Task<DeleteSubarticleResponse> Handle(DeleteSubarticleCommand request, CancellationToken cancellationToken)
            {
                Subarticle subarticle = await _context.Subarticles.Where(s => s.Id == request.SubarticleId).SingleOrDefaultAsync();

                if (subarticle == null)
                {
                    return new DeleteSubarticleResponse
                    {
                        IsSuccessful = false,
                        Message = "No subarticle found",
                    };
                }
                else
                {
                    if (request.UserRoles.Contains(subarticle.Article.EditRoleString))
                    {
                        _context.Subarticles.Remove(subarticle);
                        await _context.SaveChangesAsync();
                        return new DeleteSubarticleResponse
                        {
                            IsSuccessful = true,
                            Id = subarticle.Id,
                        };
                    }
                    else
                    {
                        return new DeleteSubarticleResponse
                        {
                            IsSuccessful = false,
                            Message = "Can delete this subarticle",
                        };
                    }
                }
            }
        }

    }
}
