using Core.Persistence.Repositories;
using TechCareer.Models.Entities;

namespace Core.Security.Repositories.Abstracts;

public interface IEventRepository : IAsyncRepository<Event, Guid>
{

    Task<Event?> GetEventWithCategoryAsync(Guid id);

    IQueryable<Event> GetEventsWithCategories();
}
