using Core.Persistence.Repositories;
using Core.Security.Repositories.Abstracts;
using TechCareer.DataAccess.Contexts;
using TechCareer.Models.Entities;


namespace Core.Security.Repositories.Concretes
{
    public sealed class EventRepository : EfRepositoryBase<Event, Guid, BaseDbContext>, IEventRepository
    {
        public EventRepository(BaseDbContext context) : base(context) { }

        public IQueryable<Event> GetEventsWithCategories()
        {
            throw new NotImplementedException();
            //return DbSet.Include(e => e.Category).AsQueryable(); //ilgili kategori datası
        }

        public Task<Event?> GetEventWithCategoryAsync(Guid id)
        {
            throw new NotImplementedException();
            //return DbSet.Include(e => e.Category).FirstOrDefaultAsync(e => e.Id == id); // id ile getirme
        }
    }
}
