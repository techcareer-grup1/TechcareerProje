using AutoMapper;
using TechCareer.Models.Dtos.VideoEducations.Request;
using TechCareer.Models.Dtos.VideoEducations.Response;
using TechCareer.Models.Entities;

namespace TechCareer.Service.Mappers;

public class VideoEduMapper : Profile
{
    public VideoEduMapper()
    {
        CreateMap<VideoEducation, VideoEduResponseDto>().ReverseMap();

        CreateMap<VideoEducation, UpdateVideoEduResponseDto>();
        CreateMap<VideoEducation, CreateVideoEduResponseDto>();

        CreateMap<CreateVideoEduRequestDto, VideoEducation>()
            .ForMember(dest => dest.TotalHours, opt => opt.MapFrom(src => Math.Round(src.TotalHours, 2)));

        CreateMap<UpdateVideoEduRequestDto, VideoEducation>()
            .ForMember(dest => dest.TotalHours, opt => opt.MapFrom(src => Math.Round(src.TotalHours, 2)));
    }
}