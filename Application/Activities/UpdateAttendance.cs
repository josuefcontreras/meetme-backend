using Application.Common.Interfaces;
using Application.Common.Models;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Activities
{
    public class UpdateAttendance
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
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
                var activity = await _context.Activities
                    .Include(activity => activity.Attendees).ThenInclude(attendece => attendece.AppUser)
                    .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

                if (activity == null) return null;

                var currentUserName = _currentUserService.UserId;
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == currentUserName, cancellationToken);

                if (user == null) return null;

                var hostUserName = activity.Attendees.FirstOrDefault(a => a.IsHost)?.AppUser?.UserName;

                var attendance = activity.Attendees.FirstOrDefault(a => a.AppUser?.UserName == user.UserName);

                if (attendance != null && hostUserName == user.UserName)
                {
                    activity.IsCancelled = !activity.IsCancelled;
                }

                if (attendance != null && hostUserName != user.UserName)
                {
                    activity.Attendees.Remove(attendance);
                }

                if (attendance == null)
                {
                    var attendee = new ActivityAttendee
                    {
                        ActivityId = activity.Id,
                        Activity = activity,
                        AppUserId = user.Id,
                        AppUser = user,
                    };

                    activity.Attendees.Add(attendee);
                }

                var result = await _context.SaveChangesAsync(cancellationToken);

                return result > 0 ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Error updating attendance.");

            }
        }
    }
}
