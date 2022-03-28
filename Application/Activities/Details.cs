using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Activities
{
    public class Details
    {
        public class Query : IRequest<Result<ActivityDTO>>
        {
            public Guid Id { get; set; }
        }
        public class Handler : IRequestHandler<Query, Result<ActivityDTO>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly ICurrentUserService _currentUserService;

            public Handler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
            {
                _context = context;
                _mapper = mapper;
                _currentUserService = currentUserService;
            }
            public async Task<Result<ActivityDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var currentUserName = _currentUserService.UserId;
                var currentUser = await _context.Users.FirstOrDefaultAsync(user => user.UserName == currentUserName, cancellationToken);

                var activity = await _context.Activities
                    .ProjectTo<ActivityDTO>(_mapper.ConfigurationProvider, new { currentUserName = currentUser.UserName })
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                return Result<ActivityDTO>.Success(activity);
            }
        }
    }
}
