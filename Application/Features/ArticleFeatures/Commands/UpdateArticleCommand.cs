using Application.Features.ArticleFeatures.Responses;
using Application.Features.Common;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ArticleFeatures.Commands
{
    public class UpdateArticleCommand : IRequest<UpdateArticleResponse>
    {
        public int ArticleId { get; set; }
        public IEnumerable<string> UserRoles { get; set; }
        public string Title { get; set; } = null;
        public bool? IsPublic { get; set; } = null;
        public string TitleImagePath { get; set; } = null;
        public int? CategoryId { get; set; } = null;
        public class UpdateArticleCommandHandler : BaseHandler, IRequestHandler<UpdateArticleCommand, UpdateArticleResponse>
        {
            public UpdateArticleCommandHandler(IApplicationDbContext context) : base(context)
            {
            }


            public async Task<UpdateArticleResponse> Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
            {
                try { 
                    Article article = await _context.Articles.Where(a => a.Id == request.ArticleId).SingleOrDefaultAsync();
                    if (article == null)
                    {
                        return new UpdateArticleResponse
                        {
                            IsSuccessful = false,
                            Message = "No article found",
                        };
                    }

                    if (request.UserRoles.Contains(article.EditRoleString))
                    {

                        if (request.Title != null)
                        {
                            article.Title = request.Title;
                        }

                        if (request.IsPublic != null)
                        {
                            article.IsPublic = request.IsPublic.Value;
                        }

                        if (request.TitleImagePath != null)
                        {
                            MediaFile newTitleImage = new MediaFile
                            {
                                Path = request.TitleImagePath,
                                Type = Domain.Enums.FileType.ImageFile,
                            };
                            _context.MediaFiles.Add(newTitleImage);

                            article.TitleImage = newTitleImage;
                        }

                        if (request.CategoryId != null)
                        {
                            Category category = await _context.Categories.Where(c => c.Id == request.CategoryId).SingleOrDefaultAsync();

                            if (category == null)
                            {
                                return new UpdateArticleResponse
                                {
                                    IsSuccessful = false,
                                    Message = "No category found",
                                };
                            }
                            article.Category = category;
                        }

                        _context.Articles.Update(article);
                        await _context.SaveChangesAsync();

                        return new UpdateArticleResponse
                        {
                            IsSuccessful = true,
                            ArticleId = article.Id,
                        };
                    }
                    else
                    {
                        return new UpdateArticleResponse
                        {
                            IsSuccessful = false,
                            Message = "Can't update this",
                        };
                    }
                } catch(Exception)
                {
                    return new UpdateArticleResponse
                    {
                        IsSuccessful = false,
                        Message = "Can't do this",
                    };
                }
            }
        }

    }
}
