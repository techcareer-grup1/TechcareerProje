namespace TechCareer.Models.Dtos.Events.Response
{
    public sealed record CreateEventResponse(
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
