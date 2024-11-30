using AutoMapper;
using Core.Persistence.Extensions;
using TechCareer.Models.Dtos.Categories.Requests;
using TechCareer.Models.Dtos.Categories.Responses;
using TechCareer.Models.Entities;

namespace TechCareer.Service.Mappers
{
    public class CategoryMapper: Profile
    {
        public CategoryMapper()
        {
            CreateMap<CategoryAddRequestDto, Category>();
            CreateMap<CategoryUpdateRequestDto, Category>();
            CreateMap<Category, CategoryResponseDto>();
            CreateMap<Paginate<Category>, Paginate<CategoryResponseDto>>();
        }
    }
}
