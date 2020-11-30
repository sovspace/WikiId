using Application.Features.AccessRequstFeatures.Responses;
using Application.Features.Common;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.AccessRequstFeatures.Commands
{
    public class DeleteAccessRequestCommand : IRequest<DeleteAccessRequestResponse>
    {
        public int AccessRequstId { get; set; }
        public string IdentityUserId { get; set; }

        public class DeleteAccessRequestCommandHandler : BaseHandler, IRequestHandler<DeleteAccessRequestCommand, DeleteAccessRequestResponse>
        {
            public DeleteAccessRequestCommandHandler(IApplicationDbContext context) : base(context)
            {
            }

            public async Task<DeleteAccessRequestResponse> Handle(DeleteAccessRequestCommand request, CancellationToken cancellationToken)
            {

                try
                {
                    AccessRequest accessRequest = await _context.AccessRequests
                        .Where(a => a.Id == request.AccessRequstId)
                        .Include(a => a.Profile)
                        .Include(a => a.Article)
                        .ThenInclude(ar => ar.Creator)
                        .SingleOrDefaultAsync();


                    if (accessRequest.Article.Creator.IdentityUserId == request.IdentityUserId)
                    {
                        _context.AccessRequests.Remove(accessRequest);
                        await _context.SaveChangesAsync();

                        return new DeleteAccessRequestResponse
                        {
                            IsSuccessful = true,
                            Id = request.AccessRequstId,
                        };
                    }
                    else
                    {
                        return new DeleteAccessRequestResponse
                        {
                            IsSuccessful = false,
                            Message = "Can't delete this access request",
                        };
                    }

                }
                catch (Exception)
                {
                    return new DeleteAccessRequestResponse
                    {
                        IsSuccessful = false,
                        Message = "Can't do this",
                    };
                }


            }
        }
    }
}
