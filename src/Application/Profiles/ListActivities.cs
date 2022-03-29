using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.Profiles
{
    public class ListActivities
    {
        public class Query : IRequest<Result<PagedList<UserActivityDTO>>>
        {
            public string UserName { get; set; }
            public UserActivitiesParams Params { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PagedList<UserActivityDTO>>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public Handler(IApplicationDbContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<PagedList<UserActivityDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _context.ActivityAttendees
                    .Where(u => u.AppUser.UserName == request.UserName)
                    .OrderBy(a => a.Activity.Date)
                    .ProjectTo<UserActivityDTO>(_mapper.ConfigurationProvider)
                    .AsQueryable();

                query = request.Params.Predicate switch
                {
                    "past" => query.Where(a => a.Date <= DateTime.Now),
                    "hosting" => query.Where(a => a.HostUserName == request.UserName),
                    _ => query.Where(a => a.Date >= DateTime.Now)
                };

                var activities = await PagedList<UserActivityDTO>.CreateAsync(query, request.Params.PageNumber, request.Params.Pagesize);

                return Result<PagedList<UserActivityDTO>>.Success(activities);
            }
        }
    }
}
