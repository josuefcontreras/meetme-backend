using Application.Common.Interfaces;
using Application.Common.Models;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Application.Photos
{
    public class Add
    {
        public class Command : IRequest<Result<Photo>>
        {
            public IFormFile File { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Photo>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IPhotoService<IPhotoFolder> _profilePhotoService;
            private readonly ICurrentUserService _currentUserService;
            private readonly IPhotoFolder _profilePhotoFolder;

            public Handler(IApplicationDbContext context, IPhotoService<IPhotoFolder> profilePhotoService, ICurrentUserService currentUserService, IOptions<FileStorageFolders> options)
            {
                _context = context;
                _profilePhotoService = profilePhotoService;
                _currentUserService = currentUserService;
                _profilePhotoFolder = new ProfilePhotoFolder { FolderName = options.Value.ProfilePhotoFolderName };
            }
            public async Task<Result<Photo>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users
                    .Include(user => user.Photos)
                    .FirstOrDefaultAsync(user => user.UserName == _currentUserService.UserId);

                if (user == null) return null;

                var photoUploadResult = await _profilePhotoService.AddPhoto(request.File, _profilePhotoFolder);

                var photo = new Photo
                {
                    Id = photoUploadResult.Id,
                    Folder = photoUploadResult.Folder,
                    Url = photoUploadResult.Url,
                    IsMain = !user.Photos.Any(p => p.IsMain),
                };

                user.Photos.Add(photo);

                var result = await _context.SaveChangesAsync(cancellationToken) > 0;

                if (result) return Result<Photo>.Success(photo);

                return Result<Photo>.Failure("Problem adding photo to DB.");

            }
        }
    }
}
