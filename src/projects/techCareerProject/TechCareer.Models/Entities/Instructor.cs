

using Core.Persistence.Repositories.Entities;

namespace TechCareer.Models.Entities;

public sealed class Instructor : Entity<Guid>
{
    public string Name { get; set; } = default!;
    public string? About { get; set; }

    public List<VideoEducation>? VideoEducations { get; set; }
}
