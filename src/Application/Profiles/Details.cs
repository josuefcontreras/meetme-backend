using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Profiles
{
    public class Details
    {
        public class Query : IRequest<Result<Profile>>
        {
            public string UserName { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<Profile>>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            private readonly IMapper _mapper;

            public Handler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
            {
                _context = context;
                _currentUserService = currentUserService;
                _mapper = mapper;
            }

            public async Task<Result<Profile>> Handle(Query request, CancellationToken cancellationToken)
            {
                var currentUserName = _currentUserService.UserId;
                var currentUser = await _context.Users.FirstOrDefaultAsync(user => user.UserName == currentUserName);

                var user = await _context.Users
                    .ProjectTo<Profile>(_mapper.ConfigurationProvider, new { currentUserName = currentUser.UserName })
                    .FirstOrDefaultAsync(user => user.UserName == request.UserName);

                return Result<Profile>.Success(user);
            }
        }
    }
}
