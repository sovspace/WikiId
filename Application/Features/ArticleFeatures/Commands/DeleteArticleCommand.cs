using Application.Features.ArticleFeatures.Responses;
using Application.Features.Common;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ArticleFeatures.Commands
{
    public class DeleteArticleCommand : IRequest<DeleteArticleResponse>
    {
        public int Id { get; set; }
        public string IdentityUserId { get; set; }

        public class DeleteArticleCommandHandler : BaseHandler, IRequestHandler<DeleteArticleCommand, DeleteArticleResponse>
        {
            public DeleteArticleCommandHandler(IApplicationDbContext context) : base(context)
            {
            }

            public async Task<DeleteArticleResponse> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    Article deletedArticle = await _context.Articles
                        .Where(a => a.Id == request.Id)
                        .Include(a => a.Creator)
                        .SingleOrDefaultAsync();

                    if (deletedArticle == null)
                    {
                        return new DeleteArticleResponse
                        {
                            IsSuccessful = false,
                            Message = "No article found",
                        };
                    }
                    else
                    {
                        if (deletedArticle.Creator.IdentityUserId != request.IdentityUserId)
                        {
                            return new DeleteArticleResponse
                            {
                                IsSuccessful = false,
                                Message = "Can't delete article",
                            };
                        }
                        else
                        {
                            _context.Articles.Remove(deletedArticle);
                            await _context.SaveChangesAsync();

                            return new DeleteArticleResponse
                            {
                                IsSuccessful = true,
                                Id = deletedArticle.Id,
                                EditRole = deletedArticle.EditRoleString,
                                ViewRole = deletedArticle.ViewRoleString,
                            };
                        }
                    }
                }
                catch (Exception)
                {
                    return new DeleteArticleResponse
                    {
                        IsSuccessful = false,
                        Message = "Can't do this",
                    };
                }

            }

        }
    }
}
