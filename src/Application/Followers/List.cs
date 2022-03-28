using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Followers
{
    public class List
    {
        public class Query : IRequest<Result<List<Profiles.Profile>>>
        {
            public string Predicate { get; set; }
            public string UserName { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<Profiles.Profile>>>
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
            public async Task<Result<List<Profiles.Profile>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var currentUserName = _currentUserService.UserId;
                var currentUser = await _context.Users.FirstOrDefaultAsync(user => user.UserName == currentUserName);
                var profiles = new List<Profiles.Profile>();

                switch (request.Predicate)
                {
                    case "followers":
                        profiles = await _context.UserFollowings
                            .Where(following => following.Target.UserName == request.UserName)
                            .Select(following => following.Observer)
                            .ProjectTo<Profiles.Profile>(_mapper.ConfigurationProvider, new { currentUserName = currentUser.UserName })
                            .ToListAsync(cancellationToken);
                        break;

                    case "following":
                        profiles = await _context.UserFollowings
                            .Where(following => following.Observer.UserName == request.UserName)
                            .Select(following => following.Target)
                            .ProjectTo<Profiles.Profile>(_mapper.ConfigurationProvider, new { currentUserName = currentUser.UserName })
                            .ToListAsync(cancellationToken);
                        break;
                    default:
                        return null;
                }

                return Result<List<Profiles.Profile>>.Success(profiles);
            }
        }
    }
}
