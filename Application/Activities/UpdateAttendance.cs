using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities
                    .Include(activity => activity.Attendees).ThenInclude(attendece => attendece.AppUser)
                    .FirstOrDefaultAsync(a => a.Id == request.Id);

                if (activity == null) return null;

                var currentUserName = _userAccessor.GetUserName();
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == currentUserName);

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

                var result = await _context.SaveChangesAsync();

                return result > 0 ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Error updating attendance.");

            }
        }
    }
}
