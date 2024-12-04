

using TechCareer.Models.Dtos.VideoEducations.Response;

namespace TechCareer.Models.Dtos.Instructors.Response;

public sealed record InstructorWithVideosResponse(Guid Id,string Name,string About,List<VideoEduResponseDto> VideoEducations);

