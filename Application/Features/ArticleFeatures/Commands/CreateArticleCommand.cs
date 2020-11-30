using Application.Features.ArticleFeatures.Responses;
using Application.Features.Common;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ArticleFeatures.Commands
{
    public class CreateArticleCommand : IRequest<CreateArticleResponse>
    {
        public string Title { get; set; }
        public string TitleImagePath { get; set; }
        public string IdentityUserId { get; set; }
        public int CategoryId { get; set; }
        public bool IsPublic { get; set; }

        public class CreateArticleCommandHandler : BaseHandler, IRequestHandler<CreateArticleCommand, CreateArticleResponse>
        {
            public CreateArticleCommandHandler(IApplicationDbContext context) : base(context)
            {
            }

            public async Task<CreateArticleResponse> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
            {

                Category category = await _context.Categories.Where(c => c.Id == request.CategoryId).SingleOrDefaultAsync();

                if (category == null)
                {
                    return new CreateArticleResponse
                    {
                        IsSuccessful = false,
                        Message = "No category found"
                    };
                }

                Profile creator = await _context.Profiles.Where(p => p.IdentityUserId == request.IdentityUserId).SingleOrDefaultAsync();
                if (creator == null)
                {
                    return new CreateArticleResponse
                    {
                        IsSuccessful = false,
                        Message = "No user found"
                    };
                }

                string editRoleName = $"Edit{request.Title}";
                string viewRoleName = $"View{request.Title}";

                MediaFile titleImage = new MediaFile
                {
                    Path = request.TitleImagePath,
                    Type = Domain.Enums.FileType.ImageFile,
                };
                _context.MediaFiles.Add(titleImage);

                Article article = new Article
                {
                    Title = request.Title,
                    TitleImage = titleImage,
                    EditRoleString = editRoleName,
                    ViewRoleString = viewRoleName,
                    Category = category,
                    Creator = creator,
                    IsPublic = request.IsPublic
                };

                _context.Articles.Add(article);
                await _context.SaveChangesAsync();

                return new CreateArticleResponse
                {
                    IsSuccessful = true,
                    ArticleId = article.Id,
                    EditArticleRoleName = editRoleName,
                    ViewArticleRoleName = viewRoleName
                };
            }
        }


    }
}
