namespace TechCareer.Models.Dtos.Events.Request
{
    public record CreateEventRequest(
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
