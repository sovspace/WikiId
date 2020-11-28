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
    public class UpdateCategoryCommand : IRequest<UpdateCategoryResponse>
    {
        public string IdentityUserId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; } = null;
        public string TitleImagePath { get; set; } = null;

        public class UpdateCategoryCommandHandler : BaseHandler, IRequestHandler<UpdateCategoryCommand, UpdateCategoryResponse>
        {
            public UpdateCategoryCommandHandler(IApplicationDbContext context) : base(context)
            {
            }

            public async Task<UpdateCategoryResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
            {
                Category category = await _context.Categories
                    .Include(c => c.Creator)
                    .Where(c => c.Id == request.CategoryId).SingleOrDefaultAsync();

                if (category == null)
                {
                    return new UpdateCategoryResponse
                    {
                        IsSuccessful = false,
                        Message = "Category is not found",
                    };
                }
                else
                {
                    if (category.Creator.IdentityUserId == request.IdentityUserId)
                    {

                        if (request.Title != null)
                        {
                            category.Title = request.Title;
                        }

                        if (request.TitleImagePath != null)
                        {
                            MediaFile mediaFile = new MediaFile
                            {
                                Path = request.TitleImagePath,
                                Type = Domain.Enums.FileType.ImageFile,
                            };
                            _context.MediaFiles.Add(mediaFile);

                            category.TitleImage = mediaFile;
                        }

                        _context.Categories.Update(category);
                        await _context.SaveChangesAsync();

                        return new UpdateCategoryResponse
                        {
                            IsSuccessful = true,
                            Id = category.Id,
                        };
                    }
                    else
                    {
                        return new UpdateCategoryResponse
                        {
                            IsSuccessful = false,
                            Message = "Can't update category",
                        };
                    }
                }

            }
        }
    }
}
