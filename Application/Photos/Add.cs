using Application.Common.Interfaces;
using Application.Common.Models;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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
            private readonly IPhotoAccessor _photoAccessor;
            private readonly ICurrentUserService _currentUserService;

            public Handler(IApplicationDbContext context, IPhotoAccessor photoAccessor, ICurrentUserService currentUserService)
            {
                _context = context;
                _photoAccessor = photoAccessor;
                _currentUserService = currentUserService;
            }
            public async Task<Result<Photo>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users
                    .Include(user => user.Photos)
                    .FirstOrDefaultAsync(user => user.UserName == _currentUserService.UserId);

                if (user == null) return null;

                var photoUploadResult = await _photoAccessor.AddPhoto(request.File);

                var photo = new Photo
                {
                    Id = photoUploadResult.PublicId,
                    Url = photoUploadResult.Url,
                    IsMain = !user.Photos.Any(p => p.IsMain) ? true : false,
                };

                user.Photos.Add(photo);

                var result = await _context.SaveChangesAsync(cancellationToken) > 0;

                if (result) return Result<Photo>.Success(photo);

                return Result<Photo>.Failure("Problem adding photo to DB.");

            }
        }
    }
}
