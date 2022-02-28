using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityAttendee> ActivityAttendees { get; set; }
        public DbSet<Photo> Photos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ActivityAttendee>(x => x.HasKey(aa => new {aa.AppUserId, aa.ActivityId}));
            
            builder.Entity<ActivityAttendee>()
                .HasOne(activityAtendee => activityAtendee.AppUser)
                .WithMany(appUser => appUser.Activities)
                .HasForeignKey(activityAtendee => activityAtendee.AppUserId);

            builder.Entity<ActivityAttendee>()
                .HasOne(activityAtendee => activityAtendee.Activity)
                .WithMany(activity => activity.Attendees)
                .HasForeignKey(activityAtendee => activityAtendee.ActivityId);
        }
    }
}
