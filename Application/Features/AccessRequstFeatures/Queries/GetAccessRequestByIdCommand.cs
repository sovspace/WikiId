using Application.Features.Common;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.AccessRequstFeatures.Queries
{
    public class GetAccessRequestByIdCommand : IRequest<AccessRequest>
    {
        public string IdentityUserId { get; set; }
        public int AccessRequestId { get; set; }
        public class GetAccessRequestByIdCommandHandler : BaseHandler, IRequestHandler<GetAccessRequestByIdCommand, AccessRequest>
        {
            public GetAccessRequestByIdCommandHandler(IApplicationDbContext context) : base(context)
            {
            }

            public async Task<AccessRequest> Handle(GetAccessRequestByIdCommand request, CancellationToken cancellationToken)
            {
                AccessRequest accessRequest = await _context.AccessRequests
                    .Where(ac => ac.Id == request.AccessRequestId)
                    .Include(ac => ac.Profile)
                    .Include(ac => ac.Article)
                    .ThenInclude(a => a.Creator).SingleOrDefaultAsync();

                if (accessRequest != null && accessRequest.Article.Creator.IdentityUserId == request.IdentityUserId)
                {
                    return accessRequest;
                }
                else
                {
                    return null;
                }

            }
        }
    }
}
