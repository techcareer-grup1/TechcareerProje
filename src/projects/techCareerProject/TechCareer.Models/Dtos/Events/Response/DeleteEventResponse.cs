namespace TechCareer.Models.Dtos.Events.Response
{
    public sealed record DeleteEventResponse(
        Guid Id,
        bool IsDeleted
    );
}
