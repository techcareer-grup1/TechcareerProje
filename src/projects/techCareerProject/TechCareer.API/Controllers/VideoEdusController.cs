using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechCareer.Models.Dtos.VideoEducations.Request;
using TechCareer.Service.Abstracts;

namespace TechCareer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoEdusController(IVideoEduService videoEduService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllVideoEdus()
        {
            return Ok(await videoEduService.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetVideoEduById([FromRoute] int id)
        {
            return Ok(await videoEduService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateVideoEdu([FromBody] CreateVideoEduRequestDto createVideoEduRequestDto)
        {
            return Ok(await videoEduService.CreateAsync(createVideoEduRequestDto));
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateVideoEdu([FromBody] UpdateVideoEduRequestDto updateVideoEduRequestDto)
        {
            return Ok(await videoEduService.UpdateAsync(updateVideoEduRequestDto));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteVideoEduById([FromRoute] int id)
        {
            return Ok(await videoEduService.DeleteAsync(id));
        }
    }
}
