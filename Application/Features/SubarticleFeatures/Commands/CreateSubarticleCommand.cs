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
    public class CreateSubarticleCommand : IRequest<CreateSubarticleResponse>
    {
        public int ArticleId { get; set; }
        public IEnumerable<string> UserRoles { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public class CreateSubarticleCommandHandler : BaseHandler, IRequestHandler<CreateSubarticleCommand, CreateSubarticleResponse>
        {
            public CreateSubarticleCommandHandler(IApplicationDbContext context) : base(context)
            {
            }

            public async Task<CreateSubarticleResponse> Handle(CreateSubarticleCommand request, CancellationToken cancellationToken)
            {
                Article article = await _context.Articles.Where(a => a.Id == request.ArticleId).SingleOrDefaultAsync();

                if (article == null)
                {
                    return new CreateSubarticleResponse
                    {
                        IsSuccessful = false,
                        Message = "No article found",
                    };
                }

                if (request.UserRoles.Contains(article.EditRoleString))
                {
                    Subarticle subarticle = new Subarticle
                    {
                        Title = request.Title,
                        Content = request.Content,
                        Article = article
                    };

                    _context.Subarticles.Add(subarticle);
                    await _context.SaveChangesAsync();
                    return new CreateSubarticleResponse
                    {
                        IsSuccessful = true,
                        Id = subarticle.Id,
                    };

                }
                else
                {
                    return new CreateSubarticleResponse
                    {
                        IsSuccessful = false,
                        Message = "Can edit article",
                    };
                }
            }
        }
    }
}
