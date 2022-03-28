using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Activities
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IApplicationDbContext _context;

            public Handler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(request.Id);
                if (activity == null) return null;

                _context.Activities.Remove(activity);

                var writtenEntries = await _context.SaveChangesAsync(cancellationToken);
                if (writtenEntries == 0) return Result<Unit>.Failure("Failed to delete the activity!");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
