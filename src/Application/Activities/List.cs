using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Activities
{
    public class List
    {
        public class Query : IRequest<Result<PagedList<ActivityDTO>>>
        {
            public ActivityParams PagingParams { get; set; }
        }
        public class Handler : IRequestHandler<Query, Result<PagedList<ActivityDTO>>>
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

            public async Task<Result<PagedList<ActivityDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var currentUserName = _currentUserService.UserId;
                var currentUser = await _context.Users.FirstOrDefaultAsync(user => user.UserName == currentUserName, cancellationToken);

                var query = _context.Activities
                    .OrderBy(a => a.Date)
                    .ProjectTo<ActivityDTO>(_mapper.ConfigurationProvider, new { currentUserName = currentUser.UserName });

                if (request.PagingParams.IsGoing && !request.PagingParams.IsHost)
                {
                    query = query.Where(x => x.Attendees.Any(a => a.UserName == currentUser.UserName));
                }

                if (request.PagingParams.IsHost && !request.PagingParams.IsGoing)
                {
                    query = query.Where(x => x.HostUserName == currentUser.UserName);
                }


                var activities = await PagedList<ActivityDTO>.CreateAsync(query, request.PagingParams.PageNumber, request.PagingParams.Pagesize);

                return Result<PagedList<ActivityDTO>>.Success(activities);
            }
        }
    }

}
