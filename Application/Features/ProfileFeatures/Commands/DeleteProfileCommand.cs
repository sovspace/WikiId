using Application.Features.Common;
using Application.Features.ProfileFeatures.Responses;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ProfileFeatures.Commands
{


    public class DeleteProfileCommand : IRequest<DeleteProfileResponse>
    {
        public string IdentityUserId { get; set; }

        public class DeleteProfileCommandHandler : BaseHandler, IRequestHandler<DeleteProfileCommand, DeleteProfileResponse>
        {
            public DeleteProfileCommandHandler(IApplicationDbContext context) : base(context)
            {
            }

            public async Task<DeleteProfileResponse> Handle(DeleteProfileCommand request, CancellationToken cancellationToken)
            {
                Profile profile = await _context.Profiles.Where(p => p.IdentityUserId == request.IdentityUserId).SingleOrDefaultAsync();
                if (profile != null)
                {
                    _context.Profiles.Remove(profile);
                    await _context.SaveChangesAsync();

                    return new DeleteProfileResponse
                    {
                        IsSuccessful = true,
                        Id = profile.Id,
                    };
                } else
                {
                    return new DeleteProfileResponse
                    {
                        IsSuccessful = false,
                        Message = "Profile not found",
                    };
                }
            }
        }

    }

}
