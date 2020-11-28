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
    public class DeleteCategoryCommand : IRequest<DeleteCategoryResponse>
    {
        public string IdentityUserId { get; set; }
        public int CategoryId { get; set; }
        public class DeleteCategoryCommandHandler : BaseHandler, IRequestHandler<DeleteCategoryCommand, DeleteCategoryResponse>
        {
            public DeleteCategoryCommandHandler(IApplicationDbContext context) : base(context)
            {
            }

            public async Task<DeleteCategoryResponse> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
            {
                Category category = await _context.Categories
                    .Include(c => c.Creator)
                    .Where(c => c.Id == request.CategoryId)
                    .SingleOrDefaultAsync();

                if (category == null)
                {
                    return new DeleteCategoryResponse
                    {
                        IsSuccessful = false,
                        Message = "No categoty found"
                    };
                }
                else
                {

                    if (category.Creator.IdentityUserId == request.IdentityUserId)
                    {
                        _context.Categories.Remove(category);
                        await _context.SaveChangesAsync();
                        return new DeleteCategoryResponse
                        {
                            IsSuccessful = true,
                            Id = category.Id,
                        };
                    }
                    else
                    {
                        return new DeleteCategoryResponse
                        {
                            IsSuccessful = false,
                            Message = "Can delete this category",
                        };
                    }
                }

            }
        }

    }
}
