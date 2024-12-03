

using Core.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using TechCareer.DataAccess.Contexts;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.Models.Entities;

namespace TechCareer.DataAccess.Repositories.Concretes;

public sealed class InstructorRepository(BaseDbContext context) : EfRepositoryBase<Instructor, Guid, BaseDbContext>(context), IInstructorRepository
{
    public IQueryable<Instructor> GetInstructorWithVideos() => Context.Instructors.Include(x => x.VideoEducations).AsQueryable();


    public Task<Instructor?> GetInstructorWithVideosAsync(Guid id) => Context.Instructors.Include(x=> x.VideoEducations)
        .FirstOrDefaultAsync(x => x.Id == id);
    
}
