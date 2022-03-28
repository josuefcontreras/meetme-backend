using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ActivityAttendeeConfiguration : IEntityTypeConfiguration<ActivityAttendee>
    {
        public void Configure(EntityTypeBuilder<ActivityAttendee> builder)
        {
            builder.HasKey(aa => new { aa.AppUserId, aa.ActivityId });

            builder.HasOne(activityAtendee => activityAtendee.AppUser)
                .WithMany(appUser => appUser.Activities)
                .HasForeignKey(activityAtendee => activityAtendee.AppUserId);

            builder.HasOne(activityAtendee => activityAtendee.Activity)
                .WithMany(activity => activity.Attendees)
                .HasForeignKey(activityAtendee => activityAtendee.ActivityId);
        }
    }
}
