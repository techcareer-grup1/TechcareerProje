using AutoMapper;
using Core.AOP.Aspects;
using Core.CrossCuttingConcerns.Exceptions.ExceptionTypes;
using Core.Persistence.Extensions;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.Models.Dtos.Categories.Requests;
using TechCareer.Models.Dtos.Categories.Responses;
using TechCareer.Models.Entities;
using TechCareer.Service.Abstracts;
using TechCareer.Service.Validations.Categories;
using TechCareer.Service.Validations.Events;

namespace TechCareer.Service.Concretes
{
    public sealed class CategoryService : ICategoryService
    {

        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [ValidationAspect(typeof(CategoryAddRequestValidator))]
        [LoggerAspect]
        [ClearCacheAspect(cacheGroupKey: "GetCategories")]
        [ClearCacheAspect(cacheGroupKey: "Categories")]
        [AuthorizeAspect(roles:"Admin")]
        public async Task AddAsync(CategoryAddRequestDto dto)
        {

            Category category = _mapper.Map<Category>(dto);

            await _categoryRepository.AddAsync(category);

        }
        [ClearCacheAspect(cacheGroupKey: "GetCategories")]
        [ClearCacheAspect(cacheGroupKey: "Categories")]
        [LoggerAspect]
        [AuthorizeAspect(roles:"Admin")]
        public async Task DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetAsync(x=> x.Id == id);

            if(category is null) 
            {
                throw new BusinessException("İgili id ye göre kategori bulunamadı.");

            }
            await _categoryRepository.DeleteAsync(category,true);
        }

        [CacheAspect(cacheKeyTemplate: "GetCategoriesList", bypassCache: false, cacheGroupKey: "GetCategories")]
        public async Task<List<CategoryResponseDto>> GetAllAsync()
        {
            List<Category> categories = await _categoryRepository.GetListAsync(enableTracking:false);

            List<CategoryResponseDto> responses = _mapper.Map<List<CategoryResponseDto>>(categories);

            return responses;

        }

        [CacheAspect(cacheKeyTemplate: "CategoryList", bypassCache: false, cacheGroupKey: "Categories")]
        public async Task<Paginate<CategoryResponseDto>> GetAllPaginateAsync(int index, int size)
        {
            Paginate<Category> categories = await _categoryRepository.GetPaginateAsync(index: index, size: size,
                enableTracking: false);

            Paginate<CategoryResponseDto> responses = _mapper.Map<Paginate<CategoryResponseDto>>(categories);

            return responses;
        }

        [ClearCacheAspect(cacheGroupKey: "GetCategories")]
        [ClearCacheAspect(cacheGroupKey: "Categories")]
        [LoggerAspect]
        [ValidationAspect(typeof(CategoryUpdateRequestValidator))]
        [AuthorizeAspect(roles:"Admin")]
        public async Task UpdateAsync(CategoryUpdateRequestDto dto)
        {
            var category = _mapper.Map<Category>(dto);

            await _categoryRepository.UpdateAsync(category);
        }
    }
}
