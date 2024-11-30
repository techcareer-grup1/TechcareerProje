

using Core.CrossCuttingConcerns.Responses;
using TechCareer.Models.Dtos.Instructors.Request;
using TechCareer.Models.Dtos.Instructors.Response;

namespace TechCareer.Service.Abstracts;

public interface IInstructorService
{
    Task<ReturnModel<List<InstructorResponse>>> GetAllAsync();
    Task<ReturnModel<InstructorResponse>> GetByIdAsync(Guid id);
    Task<ReturnModel<CreateInstructorResponse>> CreateAsync(CreateInstructorRequest request);
    Task<ReturnModel<UpdateInstructorResponse>> UpdateAsync(UpdateInstructorRequest request);
    Task<ReturnModel> DeleteAsync(Guid id);
}
