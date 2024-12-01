using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechCareer.Models.Entities;

namespace TechCareer.DataAccess.Configurations
{
    public class VideoEduConfiguration : IEntityTypeConfiguration<VideoEducation>
    {
        public void Configure(EntityTypeBuilder<VideoEducation> builder)
        {
            builder.ToTable("VideoEducations");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(v => v.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(v => v.TotalHours)
                .IsRequired();


            builder.Property(v => v.IsCertified)
                .IsRequired();

            builder.Property(v => v.Level)
                .IsRequired();

            builder.Property(v => v.ImageUrl)
                .IsRequired()
                .HasMaxLength(500);


            builder.Property(v => v.ProgrammingLanguage)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(v => v.InstructorId)
                .IsRequired();

            builder.HasIndex(v => v.Title).IsUnique();

            builder.HasOne(v => v.Instructor)
               .WithMany(i => i.VideoEducations)
               .HasForeignKey(v => v.InstructorId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(v => v.Instructor).AutoInclude();
        }
    }
}
