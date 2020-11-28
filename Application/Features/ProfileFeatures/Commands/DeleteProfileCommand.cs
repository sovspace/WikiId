using Application.Features.Common;
using Application.Features.ProfileFeatures.Responses;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ProfileFeatures.Commands
{


    public class DeleteProfileCommand : IRequest<DeleteCommandResponse>
    {
        public string IdentityUserId { get; set; }

        public class DeleteProfileCommandHandler : BaseHandler, IRequestHandler<DeleteProfileCommand, DeleteCommandResponse>
        {
            public DeleteProfileCommandHandler(IApplicationDbContext context) : base(context)
            {
            }

            public async Task<DeleteCommandResponse> Handle(DeleteProfileCommand request, CancellationToken cancellationToken)
            {
                Profile profile = new Profile { IdentityUserId = request.IdentityUserId };
                _context.Profiles.Remove(profile);
                await _context.SaveChangesAsync();

                return new DeleteCommandResponse
                {
                    IsSuccessful = true,
                    Id = profile.Id,
                };
            }
        }

    }

}
