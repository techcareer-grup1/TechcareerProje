using Core.CrossCuttingConcerns.Exceptions.ExceptionTypes;
using Core.CrossCuttingConcerns.Rules;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.Models.Entities;
using TechCareer.Service.Constants;

namespace TechCareer.Service.Rules;

public sealed class VideoEduBusinessRules(IVideoEduRepository videoEduRepository) : BaseBusinessRules
{
    public Task IsVideoEduNull(VideoEducation? videoEdu)
    {
        if (videoEdu == null)
        {
            throw new BusinessException(VideoEduMassages.VideoDoesNotExist);
        }
        return Task.CompletedTask;
    }

    public async Task IsVideoEduExist(int id)
    {
        bool isVideoEduExist = await videoEduRepository.AnyAsync(predicate:x => x.Id==id,enableTracking: false);
        if (isVideoEduExist)
        {
            throw new BusinessException(VideoEduMassages.VideoEduAlreadyAdded);
        }
    }
    
    public async Task IsVideoTitleExist(string videoTitle)
    {
        bool isVideoEduExist = await videoEduRepository.AnyAsync(predicate:x => x.Title==videoTitle,enableTracking: false);
        if (isVideoEduExist)
        {
            throw new BusinessException(VideoEduMassages.VideoDoesNotExist);
        }
    }
    
   
}