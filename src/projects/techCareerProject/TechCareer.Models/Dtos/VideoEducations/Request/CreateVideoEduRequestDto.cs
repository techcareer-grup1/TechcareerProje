using TechCareer.Models.Enums;

namespace TechCareer.Models.Dtos.VideoEducations.Request;

public sealed record CreateVideoEduRequestDto(
    string Title,
    string Description,
    double TotalHours,
    Boolean IsCertified,
    Level Level,
    string ImageUrl,
    Guid InstructorId,
    string ProgrammingLanguage
    );
