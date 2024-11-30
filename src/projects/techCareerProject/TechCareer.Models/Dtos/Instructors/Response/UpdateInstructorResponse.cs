

namespace TechCareer.Models.Dtos.Instructors.Response;

public sealed record UpdateInstructorResponse(
    Guid Id,
    string Name,
    string About,
    DateTime UpdatedDate
    );

