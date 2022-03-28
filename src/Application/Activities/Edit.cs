using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;

namespace Application.Activities
{
    public class Edit
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
            private readonly IMapper _mapper;

            public Handler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(request.Activity.Id);

                if (activity == null) return null;

                _mapper.Map(request.Activity, activity);

                var writtenEntries = await _context.SaveChangesAsync(cancellationToken);

                if (writtenEntries == 0) return Result<Unit>.Failure("Failed to updated Activity");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
