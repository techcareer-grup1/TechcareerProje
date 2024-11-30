using Core.Persistence.Repositories.Entities;

namespace TechCareer.Models.Entities;

public sealed class Category : Entity<int>
{
    public string Name { get; set; }
    public List<Event> Events { get; set; }
}
