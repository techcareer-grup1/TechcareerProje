
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechCareer.Models.Entities;

namespace TechCareer.DataAccess.Configurations;

public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
{
    public void Configure(EntityTypeBuilder<Instructor> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(128);


        builder.Property(x => x.About)
            .IsRequired()
            .HasMaxLength(2000);

        //ilişikisel conf

        builder.HasMany(i => i.VideoEducations)
              .WithOne(v => v.Instructor)
              .HasForeignKey(v => v.InstructorId)
              .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(a => a.VideoEducations).AutoInclude();

    }
}
