using Core.CrossCuttingConcerns.Responses;
using TechCareer.Models.Dtos.Events.Request;
using TechCareer.Models.Dtos.Events.Response;

namespace TechCareer.Service.Abstracts
{
    public interface IEventService
    {
        Task<ReturnModel<List<EventResponse>>> GetAllAsync();
        Task<ReturnModel<EventResponse>> GetByIdAsync(Guid id);
        Task<ReturnModel<CreateEventResponse>> CreateAsync(CreateEventRequest request);
        Task<ReturnModel<UpdateEventResponse>> UpdateAsync(UpdateEventRequest request);
        Task<ReturnModel> DeleteAsync(Guid id);
    }
}
