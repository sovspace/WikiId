using Application.Features.CategoryFeatures.Responses;
using Application.Features.Common;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.CategoryFeatures.Commands
{
    public class CreateCategoryCommand : IRequest<CreateCategoryResponse>
    {
        public string IdentityUserId { get; set; }
        public string Title { get; set; }
        public string TitleImagePath { get; set; }

        public class CreateCategoryCommandHandler : BaseHandler, IRequestHandler<CreateCategoryCommand, CreateCategoryResponse>
        {
            public CreateCategoryCommandHandler(IApplicationDbContext context) : base(context)
            {
            }

            public async Task<CreateCategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
            {
                MediaFile titleImage = new MediaFile
                {
                    Path = request.TitleImagePath,
                    Type = Domain.Enums.FileType.ImageFile,
                };
                _context.MediaFiles.Add(titleImage);

                Profile profile = await _context.Profiles.Where(p => p.IdentityUserId == request.IdentityUserId).SingleOrDefaultAsync();

                if (profile == null)
                {
                    return new CreateCategoryResponse
                    {
                        IsSuccessful = false,
                        Message = "No profile found",
                    };
                }

                Category category = new Category
                {
                    Creator = profile,
                    Title = request.Title,
                    TitleImage = titleImage
                };

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                return new CreateCategoryResponse
                {
                    IsSuccessful = true,
                    Id = category.Id,
                };
            }
        }
    }
}
