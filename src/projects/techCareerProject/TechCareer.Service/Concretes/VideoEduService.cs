using System.Net;
using AutoMapper;
using Core.AOP.Aspects;
using Core.CrossCuttingConcerns.Responses;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.Models.Dtos.VideoEducations.Request;
using TechCareer.Models.Dtos.VideoEducations.Response;
using TechCareer.Models.Entities;
using TechCareer.Service.Abstracts;
using TechCareer.Service.Constants;
using TechCareer.Service.EventsRabbitMQ;
using TechCareer.Service.RabbitMQ;
using TechCareer.Service.Rules;
using TechCareer.Service.Validations.Instructors;
using TechCareer.Service.Validations.VideoEducations;

namespace TechCareer.Service.Concretes;

public sealed class VideoEduService(IVideoEduRepository videoEduRepository, VideoEduBusinessRules videoEduBusinessRules,
IMapper mapper,RabbitMQPublisher rabbitMQPublisher) : IVideoEduService
{
    [CacheAspect(cacheKeyTemplate: "GetVideoEducationList", bypassCache: false, cacheGroupKey: "GetVideoEducation")]
    public async Task<ReturnModel<List<VideoEduResponseDto>>> GetAllAsync()
    {
        List<VideoEducation> videoEducations = await videoEduRepository.GetListAsync();
        
        List<VideoEduResponseDto> videoEduResponses = mapper.Map<List<VideoEduResponseDto>>(videoEducations);
        
        return ReturnModel<List<VideoEduResponseDto>>.Success(videoEduResponses,VideoEduMassages.VideoEduListed);
    }

    public async Task<ReturnModel<VideoEduResponseDto>> GetByIdAsync(int id)
    {
        VideoEducation? videoEducation = await videoEduRepository.GetAsync(x => x.Id == id);
        await videoEduBusinessRules.IsVideoEduNull(videoEducation);
        
        VideoEduResponseDto videoEduResponseDto = mapper.Map<VideoEduResponseDto>(videoEducation);
        
        return ReturnModel<VideoEduResponseDto>.Success(videoEduResponseDto,VideoEduMassages.VideoEduListedById);
    }

    [ValidationAspect(typeof(CreateVideoEduRequestValidator))]
    [LoggerAspect]
    [ClearCacheAspect(cacheGroupKey: "GetVideoEducation")]
    [AuthorizeAspect(roles:"Admin")]
    public async Task<ReturnModel<CreateVideoEduResponseDto>> CreateAsync(CreateVideoEduRequestDto request)
    {
        await videoEduBusinessRules.IsVideoTitleExist(request.Title);
        
        VideoEducation videoEducation = mapper.Map<VideoEducation>(request);
        await videoEduRepository.AddAsync(videoEducation);

        var videoEducationImageCreatedEvent = new VideoEducationImageCreatedEvent
        {
            ImageUrl = videoEducation.ImageUrl 
        };

        rabbitMQPublisher.Publish(videoEducationImageCreatedEvent);


        CreateVideoEduResponseDto videoEduResponseDto = mapper.Map<CreateVideoEduResponseDto>(videoEducation);
        return ReturnModel<CreateVideoEduResponseDto>.Success(videoEduResponseDto,VideoEduMassages.VideoEduAdded,HttpStatusCode.Created);
    }

    [ClearCacheAspect(cacheGroupKey: "GetVideoEducation")]
    [LoggerAspect]
    [ValidationAspect(typeof(UpdateVideoEduRequestValidator))]
    [AuthorizeAspect(roles:"Admin")]
    public  async Task<ReturnModel<UpdateVideoEduResponseDto>> UpdateAsync(UpdateVideoEduRequestDto request)
    {
        await videoEduBusinessRules.IsVideoEduExist(request.Id);
        
        VideoEducation videoEducation = mapper.Map<VideoEducation>(request);
        await videoEduRepository.UpdateAsync(videoEducation);
        
        UpdateVideoEduResponseDto videoEduResponseDto = mapper.Map<UpdateVideoEduResponseDto>(videoEducation);
        
        return ReturnModel<UpdateVideoEduResponseDto>.Success(videoEduResponseDto,VideoEduMassages.VideoEduUpdated);
        
    }

    [ClearCacheAspect(cacheGroupKey: "GetVideoEducation")]
    [LoggerAspect]
    [AuthorizeAspect(roles:"Admin")]
    public async Task<ReturnModel> DeleteAsync(int id)
    {
        VideoEducation? videoEducation = await videoEduRepository.GetAsync(x => x.Id == id);
        await videoEduBusinessRules.IsVideoEduNull(videoEducation);
        await videoEduRepository.DeleteAsync(videoEducation);
        
        return ReturnModel.Success(VideoEduMassages.VideoEduDeleted,HttpStatusCode.NoContent);
        
    }
}