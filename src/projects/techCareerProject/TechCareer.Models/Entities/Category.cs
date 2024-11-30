using Core.Persistence.Repositories.Entities;
using TechCareer.Models.Entities;

namespace TechCareer.Models.Entities
{
    public class Category : Entity<int>
    {
        public string Name { get; set; }

        public virtual ICollection<Event> Events { get; set; } = null!;

        public Category()
        {
            Name = string.Empty;
        }

        public Category(string name)
        {
            Name = name;
        }

        public Category(int id, string name) : base(id)
        {
            Name = name;
        }
    }
}
