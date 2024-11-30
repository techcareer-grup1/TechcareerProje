using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechCareer.Models.Dtos.Categories.Requests;
using TechCareer.Service.Abstracts;

namespace TechCareer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoryService categoryService) : ControllerBase
    {
        [HttpGet("GetAll")]
       
        public async Task<IActionResult> GetAll() 
        {
        
            var result=await categoryService.GetAllAsync();
            return Ok(result);
        
        
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(CategoryAddRequestDto dto) 
        {

            await categoryService.AddAsync(dto);

            return Ok(dto);
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(int id)
        {

            await categoryService.DeleteAsync(id);

            return Ok(id);
        }

    }
}
