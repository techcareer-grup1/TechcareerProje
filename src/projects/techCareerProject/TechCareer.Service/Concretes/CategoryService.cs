using AutoMapper;
using Core.CrossCuttingConcerns.Exceptions.ExceptionTypes;
using Core.Persistence.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.Models.Dtos.Categories.Requests;
using TechCareer.Models.Dtos.Categories.Responses;
using TechCareer.Models.Entities;
using TechCareer.Service.Abstracts;

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


        public async Task AddAsync(CategoryAddRequestDto dto)
        {

            Category category = _mapper.Map<Category>(dto);

            await _categoryRepository.AddAsync(category);

        }

        public async Task DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetAsync(x=> x.Id == id);

            if(category is null) 
            {
                throw new BusinessException("İgili id ye göre kategori bulunamadı.");

            }
            await _categoryRepository.DeleteAsync(category);
        }

        public async Task<List<CategoryResponseDto>> GetAllAsync()
        {
            List<Category> categories = await _categoryRepository.GetListAsync(enableTracking:false);

            List<CategoryResponseDto> responses = _mapper.Map<List<CategoryResponseDto>>(categories);

            return responses;

        }

        public async Task<Paginate<CategoryResponseDto>> GetAllPaginateAsync(int index, int size)
        {
            Paginate<Category> categories = await _categoryRepository.GetPaginateAsync(index: index, size: size,
                enableTracking: false);

            Paginate<CategoryResponseDto> responses = _mapper.Map<Paginate<CategoryResponseDto>>(categories);

            return responses;
        }

        public async Task UpdateAsync(CategoryUpdateRequestDto dto)
        {
            var category = _mapper.Map<Category>(dto);

            await _categoryRepository.UpdateAsync(category);
        }
    }
}
