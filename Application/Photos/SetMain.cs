using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Photos
{
    public class SetMain
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public Handler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users
                    .Include(user => user.Photos)
                    .FirstOrDefaultAsync(user => user.UserName == _currentUserService.UserId);

                if (user == null) return null;

                var photo = user.Photos.FirstOrDefault(photo => photo.Id == request.Id);

                if (photo == null) return null;

                var currentMain = user.Photos.FirstOrDefault(photo => photo.IsMain);

                if (currentMain != null) currentMain.IsMain = false;

                photo.IsMain = true;

                var sucess = await _context.SaveChangesAsync(cancellationToken) > 0;

                if (sucess) return Result<Unit>.Success(Unit.Value);

                return Result<Unit>.Failure("Failed to set main photo in database");

            }
        }
    }
}
