using Application.Features.Common;
using Application.Features.MediaFileFeatures.Responses;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.MediaFileFeatures.Commands
{
    public class CreateMediaFileCommand : IRequest<CreateMediaFileResponse>
    {
        public string Path { get; set; }
        public FileType FileType { get; set; }
        public class CreateMediaFileCommandHandler : BaseHandler, IRequestHandler<CreateMediaFileCommand, CreateMediaFileResponse>
        {
            public CreateMediaFileCommandHandler(IApplicationDbContext context) : base(context)
            {
            }

            public async Task<CreateMediaFileResponse> Handle(CreateMediaFileCommand request, CancellationToken cancellationToken)
            {
                MediaFile mediaFile = new MediaFile
                {
                    Path = request.Path,
                    Type = request.FileType,
                };

                _context.MediaFiles.Add(mediaFile);
                await _context.SaveChangesAsync();

                return new CreateMediaFileResponse
                {
                    IsSuccessful = true,
                    Id = mediaFile.Id,
                };
            }
        }
    }
}
