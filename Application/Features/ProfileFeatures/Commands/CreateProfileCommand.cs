using Application.Features.Common;
using Application.Features.ProfileFeatures.Responses;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ProfileFeatures.Commands
{
    public class CreateProfileCommand : IRequest<CreateProfileResponse>
    {
        public string IdentityUserId { get; set; }

        public class CreateProfileCommandHandler : BaseHandler, IRequestHandler<CreateProfileCommand, CreateProfileResponse>
        {
            public CreateProfileCommandHandler(IApplicationDbContext context) : base(context)
            {
            }

            public async Task<CreateProfileResponse> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
            {
                Profile profile = new Profile { IdentityUserId = request.IdentityUserId };
                _context.Profiles.Add(profile);
                await _context.SaveChangesAsync();

                return new CreateProfileResponse
                {
                    IsSuccessful = true,
                    Id = profile.Id,
                };

            }
        }

    }
}
