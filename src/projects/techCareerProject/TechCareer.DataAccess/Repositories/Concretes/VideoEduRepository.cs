using Core.Persistence.Repositories;
using TechCareer.DataAccess.Contexts;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.Models.Entities;

namespace TechCareer.DataAccess.Repositories.Concretes;

public class VideoEduRepository(BaseDbContext context) : EfRepositoryBase<VideoEducation,int,BaseDbContext>(context), IVideoEduRepository
{
    
}