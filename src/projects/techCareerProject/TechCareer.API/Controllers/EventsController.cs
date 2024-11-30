using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechCareer.Models.Dtos.Events.Request;
using TechCareer.Service.Abstracts;

namespace TechCareer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEvents() => Ok(await _eventService.GetAllAsync());

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetEventById([FromRoute] Guid id) => Ok(await _eventService.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request) => Ok(await _eventService.CreateAsync(request));

        [HttpPut]
        public async Task<IActionResult> UpdateEvent([FromBody] UpdateEventRequest request) => Ok(await _eventService.UpdateAsync(request));

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEvent([FromRoute] Guid id) => Ok(await _eventService.DeleteAsync(id));
    }
}
