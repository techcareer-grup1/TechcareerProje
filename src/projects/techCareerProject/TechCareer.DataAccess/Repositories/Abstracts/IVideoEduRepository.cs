using Core.Persistence.Repositories;
using TechCareer.Models.Entities;

namespace TechCareer.DataAccess.Repositories.Abstracts;

public interface IVideoEduRepository : IAsyncRepository<VideoEducation, int>
{
    
}