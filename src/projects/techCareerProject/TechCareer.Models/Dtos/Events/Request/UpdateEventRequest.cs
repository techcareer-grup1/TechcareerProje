namespace TechCareer.Models.Dtos.Events.Request
{
    public record UpdateEventRequest(
        Guid Id,
        string Title,
        string Description,
        string ImageUrl,
        DateTime StartDate,
        DateTime EndDate,
        DateTime ApplicationDeadline,
        string ParticipationText,
        int CategoryId
    );
}
