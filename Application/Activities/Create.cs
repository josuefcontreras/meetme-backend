using Application.Common.Interfaces;
using Application.Common.Models;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Activities
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Activity Activity { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
            }
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

                var user = await _context.Users.FirstOrDefaultAsync(user => user.UserName == currentUserName, cancellationToken);

                var attendee = new ActivityAttendee
                {
                    AppUserId = user.Id,
                    AppUser = user,
                    ActivityId = request.Activity.Id,
                    Activity = request.Activity,
                    IsHost = true
                };

                request.Activity.Attendees.Add(attendee);

                _context.Activities.Add(request.Activity);

                var writtenEntries = await _context.SaveChangesAsync(cancellationToken);

                if (writtenEntries == 0) return Result<Unit>.Failure("Failed to create activity!");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
