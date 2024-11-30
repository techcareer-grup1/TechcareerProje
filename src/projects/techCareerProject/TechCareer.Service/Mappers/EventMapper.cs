using AutoMapper;
using TechCareer.Models.Dtos.Events.Request;
using TechCareer.Models.Dtos.Events.Response;
using TechCareer.Models.Entities;

namespace TechCareer.Service.Mappers
{
    public class EventMapper : Profile
    {
        public EventMapper()
        {
            // Map between Event and EventResponse
            CreateMap<Event, EventResponse>().ReverseMap();

            // Map between CreateEventRequest and Event
            CreateMap<CreateEventRequest, Event>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.ApplicationDeadline, opt => opt.MapFrom(src => src.ApplicationDeadline))
                .ForMember(dest => dest.ParticipationText, opt => opt.MapFrom(src => src.ParticipationText))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));

            // Map between UpdateEventRequest and Event
            CreateMap<UpdateEventRequest, Event>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.ApplicationDeadline, opt => opt.MapFrom(src => src.ApplicationDeadline))
                .ForMember(dest => dest.ParticipationText, opt => opt.MapFrom(src => src.ParticipationText))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));

            // Map between Event and CreateEventResponse
            CreateMap<Event, CreateEventResponse>();

            // Map between Event and UpdateEventResponse
            CreateMap<Event, UpdateEventResponse>();
        }
    }
}
