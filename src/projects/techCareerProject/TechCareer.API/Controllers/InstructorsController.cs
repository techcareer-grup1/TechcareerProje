using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechCareer.Models.Dtos.Instructors.Request;
using TechCareer.Service.Abstracts;

namespace TechCareer.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InstructorsController(IInstructorService instructorService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllInstructors() => Ok(await instructorService.GetAllAsync());

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetInstructorById([FromRoute] Guid id) => Ok(await instructorService.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> CreateInstructor([FromBody] CreateInstructorRequest request) => Ok(await instructorService
    .CreateAsync(request));
    [HttpPut]
    public async Task<IActionResult> UpdateInstructor([FromBody] UpdateInstructorRequest request) =>
    Ok(await instructorService.UpdateAsync(request));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteInstructor([FromRoute] Guid id) => Ok(await instructorService.DeleteAsync(id));
}
