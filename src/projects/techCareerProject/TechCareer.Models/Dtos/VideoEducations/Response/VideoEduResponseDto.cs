using TechCareer.Models.Enums;

namespace TechCareer.Models.Dtos.VideoEducations.Response;

public record VideoEduResponseDto(
    string Title,
    string Description,
    double TotalHours,
    Boolean IsCertified,
    Level Level,
    string ImageUrl,
    Guid InstructorId,
    string ProgrammingLanguage
    );