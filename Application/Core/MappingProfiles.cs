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

            CreateMap<ActivityAttendee, AttendeeDTO>()
                .ForMember(attendeeDTO => attendeeDTO.DisplayName, memberOptions => memberOptions
                    .MapFrom(attendee => attendee.AppUser.DisplayName))
                .ForMember(attendeeDTO => attendeeDTO.UserName, memberOptions => memberOptions
                    .MapFrom(attendee => attendee.AppUser.UserName))
                .ForMember(attendeeDTO => attendeeDTO.Bio, memberOptions => memberOptions
                    .MapFrom(attendee => attendee.AppUser.Bio))
                .ForMember(attendeeDTO => attendeeDTO.Image, memberOptions => memberOptions
                    .MapFrom(attendee => attendee.AppUser.Photos.FirstOrDefault(photo => photo.IsMain).Url));

            CreateMap<AppUser, Profiles.Profile>()
                .ForMember(profile => profile.Image, memberOptions  => memberOptions
                    .MapFrom(appUser => appUser.Photos.FirstOrDefault(photo => photo.IsMain).Url));
        }

    }
}
