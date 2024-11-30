

namespace TechCareer.Models.Dtos.Instructors.Response;

public sealed record InstructorResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string About { get; init; }
    public DateTime CreatedDate { get; init; }
    public DateTime UpdatedDate { get; init; }

    public InstructorResponse() {}
   
    public InstructorResponse(Guid id,string name,string about, DateTime createdDate,DateTime updatedDate)
    {
        Id = id;
        Name = name;
        About = about;
        CreatedDate = createdDate;
        UpdatedDate = updatedDate;
    }
}
