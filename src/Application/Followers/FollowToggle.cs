using Application.Common.Interfaces;
using Application.Common.Models;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Followers
{
    public class FollowToggle
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string TargetUserName { get; set; }
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
                var currentUserName = _currentUserService.UserId;
                var observer = await _context.Users.FirstOrDefaultAsync(user => user.UserName == currentUserName, cancellationToken);

                var target = await _context.Users.FirstOrDefaultAsync(user => user.UserName == request.TargetUserName);
                if (target == null) return null;

                if (target.UserName == observer.UserName) return Result<Unit>.Failure("You can not follow yourself.");

                var following = await _context.UserFollowings.FindAsync(observer.Id, target.Id);

                if (following == null) // not following target. Should add following table UserFollowings
                {
                    var userFollowing = new UserFollowing { Observer = observer, Target = target };
                    _context.UserFollowings.Add(userFollowing);

                }
                else
                {
                    _context.UserFollowings.Remove(following);
                }

                var success = await _context.SaveChangesAsync(cancellationToken) > 0;

                if (success) return Result<Unit>.Success(Unit.Value);

                return Result<Unit>.Failure("Failed to update Following.");

            }
        }
    }
}
