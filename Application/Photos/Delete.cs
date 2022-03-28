using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Photos
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            private readonly IPhotoAccessor _photoAccessor;

            public Handler(IApplicationDbContext context, ICurrentUserService currentUserService, IPhotoAccessor photoAccessor)
            {
                _context = context;
                _currentUserService = currentUserService;
                _photoAccessor = photoAccessor;
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users
                    .Include(user => user.Photos)
                    .FirstOrDefaultAsync(user => user.UserName == _currentUserService.UserId);

                if (user == null) return null;

                var photo = user.Photos.FirstOrDefault(photo => photo.Id == request.Id);

                if (photo == null) return null;

                if (photo.IsMain) return Result<Unit>.Failure("You can not delete your main photo");

                var deleteResult = await _photoAccessor.DeletPhoto(request.Id);

                if (deleteResult == null) Result<Unit>.Failure("Failed to delete photo from storage");

                _context.Photos.Remove(photo);

                var result = await _context.SaveChangesAsync(cancellationToken) > 0;

                if (result) return Result<Unit>.Success(Unit.Value);

                return Result<Unit>.Failure("Failed to delete photo from database");

                throw new NotImplementedException();
            }
        }
    }
}
