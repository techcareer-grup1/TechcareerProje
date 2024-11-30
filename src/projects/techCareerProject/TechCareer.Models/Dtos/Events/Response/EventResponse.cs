namespace TechCareer.Models.Dtos.Events.Response
{
    public sealed record EventResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public string ImageUrl { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public DateTime ApplicationDeadline { get; init; }
        public string ParticipationText { get; init; }
        public int CategoryId { get; init; }
        public DateTime CreatedDate { get; init; }
        public DateTime UpdatedDate { get; init; }

        public EventResponse() { }

        public EventResponse(
            Guid id,
            string title,
            string description,
            string imageUrl,
            DateTime startDate,
            DateTime endDate,
            DateTime applicationDeadline,
            string participationText,
            int categoryId,
            DateTime createdDate,
            DateTime updatedDate)
        {
            Id = id;
            Title = title;
            Description = description;
            ImageUrl = imageUrl;
            StartDate = startDate;
            EndDate = endDate;
            ApplicationDeadline = applicationDeadline;
            ParticipationText = participationText;
            CategoryId = categoryId;
            CreatedDate = createdDate;
            UpdatedDate = updatedDate;
        }
    }
}
