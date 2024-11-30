using System.Linq.Expressions;
using Core.CrossCuttingConcerns.Responses;
using Core.Persistence.Extensions;
using TechCareer.Models.Dtos.VideoEducations.Request;
using TechCareer.Models.Dtos.VideoEducations.Response;
using TechCareer.Models.Entities;

namespace TechCareer.Service.Abstracts;

public interface IVideoEduService
{
    Task<ReturnModel<List<VideoEduResponseDto>>> GetAllAsync();
    Task<ReturnModel<VideoEduResponseDto>> GetByIdAsync(int id);
    Task<ReturnModel<CreateVideoEduResponseDto>> CreateAsync(CreateVideoEduRequestDto request);
    Task<ReturnModel<UpdateVideoEduResponseDto>> UpdateAsync(UpdateVideoEduRequestDto request);
    Task<ReturnModel> DeleteAsync(int id);
}
