using Application.Features.AccessRequstFeatures.Responses;
using Application.Features.Common;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.AccessRequstFeatures.Commands
{
    public class CreateAccessRequestCommand : IRequest<CreateAccessRequestResponse>
    {
        public string IdentityUserId { get; set; }
        public IEnumerable<string> UserRoles { get; set; }
        public int ArticleId { get; set; }
        public AccessType AccessType { get; set; }
        public class CreateAccessRequestCommandHandler : BaseHandler, IRequestHandler<CreateAccessRequestCommand, CreateAccessRequestResponse>
        {
            public CreateAccessRequestCommandHandler(IApplicationDbContext context) : base(context)
            {
            }

            public async Task<CreateAccessRequestResponse> Handle(CreateAccessRequestCommand request, CancellationToken cancellationToken)
            {
                Article article = await _context.Articles.Where(a => a.Id == request.ArticleId).SingleOrDefaultAsync();
                if (article == null)
                {
                    return new CreateAccessRequestResponse
                    {
                        IsSuccessful = false,
                        Message = "Article not found",
                    };
                }

                if ((request.AccessType == AccessType.Edit && request.UserRoles.Contains(article.EditRoleString)) ||
                    (request.AccessType == AccessType.View && request.UserRoles.Contains(article.ViewRoleString)))
                {
                    return new CreateAccessRequestResponse
                    {
                        IsSuccessful = false,
                        Message = "You have already this role",
                    };
                }


                Profile profile = await _context.Profiles.Where(p => p.IdentityUserId == request.IdentityUserId).SingleOrDefaultAsync();
                if (profile == null)
                {
                    return new CreateAccessRequestResponse
                    {
                        IsSuccessful = false,
                        Message = "Profile not found",
                    };
                }

                AccessRequest accessRequest = new AccessRequest
                {
                    AccessType = request.AccessType,
                    Profile = profile,
                    Article = article,
                };

                _context.AccessRequests.Add(accessRequest);
                await _context.SaveChangesAsync();

                return new CreateAccessRequestResponse
                {
                    IsSuccessful = true,
                    Id = accessRequest.Id,
                };

            }
        }
    }
}
