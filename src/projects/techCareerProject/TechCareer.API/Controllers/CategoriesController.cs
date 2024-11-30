using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }
}
