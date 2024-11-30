using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechCareer.Models.Entities;

namespace TechCareer.DataAccess.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("Events").HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("Id").IsRequired();
        builder.Property(e => e.Title).HasColumnName("Title").IsRequired().HasMaxLength(256); // Adjust max length if needed
        builder.Property(e => e.Description).HasColumnName("Description").IsRequired().HasMaxLength(2000);
        builder.Property(e => e.ImageUrl).HasColumnName("ImageUrl").IsRequired().HasMaxLength(512);
        builder.Property(e => e.StartDate).HasColumnName("StartDate").IsRequired();
        builder.Property(e => e.EndDate).HasColumnName("EndDate").IsRequired();
        builder.Property(e => e.ApplicationDeadline).HasColumnName("ApplicationDeadline").IsRequired();
        builder.Property(e => e.ParticipationText).HasColumnName("ParticipationText").IsRequired().HasMaxLength(2000);
        builder.Property(e => e.CategoryId).HasColumnName("CategoryId").IsRequired();

        // Relationship configuration
        builder.HasOne(e => e.Category)
               .WithMany()
               .HasForeignKey(e => e.CategoryId)
               .OnDelete(DeleteBehavior.Cascade);

        // Optional: Seeding example data
        builder.HasData(GetSeeds());
    }

    private IEnumerable<Event> GetSeeds()
    {
        return new List<Event>
        {
            new Event
            {
                Id = Guid.NewGuid(),
                Title = "SeedData Title",
                Description = "SeedData Description .... AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                ImageUrl = "",
                StartDate = new DateTime(2024, 5, 1),
                EndDate = new DateTime(2024, 5, 3),
                ApplicationDeadline = new DateTime(2024, 4, 20),
                ParticipationText = "SeedData Text AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                CategoryId = 1
            }
        };
    }
}
