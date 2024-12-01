using Core.Persistence.Repositories.Entities;
using TechCareer.Models.Enums;

namespace TechCareer.Models.Entities;

public class VideoEducation : Entity<int>
{
    public VideoEducation()
    {
        Title=string.Empty;
        Description=string.Empty;
        ImageUrl=string.Empty;
        ProgrammingLanguage=string.Empty;
    }

    public VideoEducation(string title, string description, double totalHours, bool ısCertified, Level level, string ımageUrl, Guid ınstructorId, string programmingLanguage)
    {
        Title = title;
        Description = description;
        TotalHours = totalHours;
        IsCertified = ısCertified;
        Level = level;
        ImageUrl = ımageUrl;
        InstructorId = ınstructorId;
        ProgrammingLanguage = programmingLanguage;
    }

    public VideoEducation(int id, string title, string description, double totalHours, bool ısCertified, Level level, string ımageUrl, Guid ınstructorId, string programmingLanguage)
    :base(id)
    {
        Title = title;
        Description = description;
        TotalHours = totalHours;
        IsCertified = ısCertified;
        Level = level;
        ImageUrl = ımageUrl;
        InstructorId = ınstructorId;
        ProgrammingLanguage = programmingLanguage;
    }
    public string Title { get; set; }
    public string Description { get; set; }
    public double TotalHours { get; set; }
    public Boolean IsCertified { get; set; }
    public Level Level { get; set; }
    public string ImageUrl { get; set; }
    public Instructor Instructor { get; set; }
    public Guid InstructorId { get; set; }
    public string ProgrammingLanguage { get; set; }
}