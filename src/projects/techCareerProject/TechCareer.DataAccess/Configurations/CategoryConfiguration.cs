using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechCareer.Models.Entities;

namespace TechCareer.DataAccess.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories").HasKey(x=> x.Id );
            builder.Property(o => o.Name).HasColumnName("CategoryName");
            builder.Property(u => u.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            builder.Property(u => u.UpdatedDate).HasColumnName("UpdatedDate");
            builder.Property(u => u.DeletedDate).HasColumnName("DeletedDate");

            builder.HasMany(o => o.Events).WithOne(x=> x.Category).HasForeignKey(x=>x.CategoryId).OnDelete(DeleteBehavior.Cascade); ;

            builder.HasData(new Category { Id = 1, Name = "Hackathon" });

        }
    }
}
