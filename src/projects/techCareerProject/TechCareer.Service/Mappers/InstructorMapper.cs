

using AutoMapper;
using TechCareer.Models.Dtos.Instructors.Request;
using TechCareer.Models.Dtos.Instructors.Response;
using TechCareer.Models.Entities;

namespace TechCareer.Service.Mappers;

public class InstructorMapper : Profile
{
    public InstructorMapper()
    {
        CreateMap<Instructor, InstructorResponse>().ReverseMap();
        CreateMap<CreateInstructorRequest, Instructor>().ForMember(des => des.Name,
            opt => opt.MapFrom(src => src.Name)).ForMember(des => des.About,
            opt => opt.MapFrom(src => src.About));
        CreateMap<UpdateInstructorRequest, Instructor>().ForMember(des => des.Name,
            opt => opt.MapFrom(src => src.Name)).ForMember(des => des.About,
            opt => opt.MapFrom(src => src.About));
        CreateMap<Instructor, CreateInstructorResponse>();
        CreateMap<Instructor,UpdateInstructorResponse>();
    }
}
