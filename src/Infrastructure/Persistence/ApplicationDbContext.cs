using Application.Common.Interfaces;
using Domain;
using Domain.Common;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>, IApplicationDbContext
    {
        private readonly IDateTime _dateTime;
        private readonly ICurrentUserService _currentUserService;

        public ApplicationDbContext(
            DbContextOptions options,
            IDateTime dateTime,
            ICurrentUserService currentUserService
            ) : base(options)
        {
            _dateTime = dateTime;
            _currentUserService = currentUserService;
        }

        public DbSet<Activity> Activities => Set<Activity>();
        public DbSet<ActivityAttendee> ActivityAttendees => Set<ActivityAttendee>();
        public DbSet<Domain.Photo> Photos => Set<Domain.Photo>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<UserFollowing> UserFollowings => Set<UserFollowing>();
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.Created = _dateTime.Now;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        entry.Entity.LastModified = _dateTime.Now;
                        break;
                }
            }

            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
        public override EntityEntry Entry(object entity)
        {
            return base.Entry(entity);
        }
    }
}
