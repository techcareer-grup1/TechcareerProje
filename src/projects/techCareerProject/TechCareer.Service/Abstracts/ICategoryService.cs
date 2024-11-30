using Core.Persistence.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechCareer.Models.Dtos.Categories.Requests;
using TechCareer.Models.Dtos.Categories.Responses;

namespace TechCareer.Service.Abstracts
{
    public interface ICategoryService
    {
        Task AddAsync(CategoryAddRequestDto dto);
        Task UpdateAsync(CategoryUpdateRequestDto dto);
        Task DeleteAsync( int id);

        Task<List<CategoryResponseDto>> GetAllAsync();

        Task<Paginate<CategoryResponseDto>> GetAllPaginateAsync(int index,int size);
    }
}
