using Application.Activities;
using AutoMapper;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Activity, Activity>();

            CreateMap<Activity, ActivityDTO>()
                .ForMember(activitiDTO => activitiDTO.HostUserName, memberOptions => memberOptions
                .MapFrom(activity => activity.Attendees
                .FirstOrDefault(attendee => attendee.IsHost).AppUser.UserName));

            CreateMap<ActivityAttendee, Profiles.Profile>()
                .ForMember(profile => profile.DisplayName, memberOptions => memberOptions
                    .MapFrom(attendee => attendee.AppUser.DisplayName))
                .ForMember(profile => profile.UserName, memberOptions => memberOptions
                    .MapFrom(attendee => attendee.AppUser.UserName))
                .ForMember(profile => profile.Bio, memberOptions => memberOptions
                    .MapFrom(attendee => attendee.AppUser.Bio));
        }

    }
}
