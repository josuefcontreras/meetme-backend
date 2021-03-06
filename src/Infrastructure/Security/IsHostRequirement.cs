using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Infrastructure.Security
{
    public class IsHostRequirement : IAuthorizationRequirement
    {


    }

    public class IsHostRequirementHandler : AuthorizationHandler<IsHostRequirement>
    {
        private readonly ApplicationDbContext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IsHostRequirementHandler(ApplicationDbContext dataContext, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsHostRequirement requirement)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null) return Task.CompletedTask;

            var activityId = _httpContextAccessor.HttpContext?.Request.RouteValues
                    .SingleOrDefault(x => x.Key == "id").Value?.ToString();

            var activityGuid = Guid.Parse(activityId!);

            var attendee = _dataContext.ActivityAttendees
                .AsNoTracking()
                .SingleOrDefaultAsync(attendee => attendee.ActivityId == activityGuid &&
                attendee.AppUserId == userId).Result;

            if (attendee == null) return Task.CompletedTask;

            if (attendee.IsHost) context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
