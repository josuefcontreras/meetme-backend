using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Activity> Activities { get; }
        public DbSet<ActivityAttendee> ActivityAttendees { get; }
        public DbSet<Photo> Photos { get; }
        public DbSet<Comment> Comments { get; }
        public DbSet<UserFollowing> UserFollowings { get; }
        public DbSet<AppUser> Users { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        public EntityEntry Entry(object entity);
    }
}
