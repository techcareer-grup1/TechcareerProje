
namespace TechCareer.Models.Dtos.Instructors.Response;

public sealed record CreateInstructorResponse(
    Guid Id,
    string Name,
    string About,
    DateTime CreatedDate
    );

