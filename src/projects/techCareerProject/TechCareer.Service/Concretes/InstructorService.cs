﻿
using AutoMapper;
using Core.AOP.Aspects;
using Core.CrossCuttingConcerns.Responses;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Net;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.Models.Dtos.Instructors.Request;
using TechCareer.Models.Dtos.Instructors.Response;
using TechCareer.Models.Entities;
using TechCareer.Service.Abstracts;
using TechCareer.Service.Constants;
using TechCareer.Service.Rules;
using TechCareer.Service.Validations.Instructors;

namespace TechCareer.Service.Concretes;

public class InstructorService(IInstructorRepository instructorRepository,InstructorBusinessRules instructorBusinessRules,IMapper mapper) : IInstructorService
{
    [ValidationAspect(typeof(CreateInstructorRequestValidator))]
    [LoggerAspect]
    [ClearCacheAspect(cacheGroupKey:"GetInstructors")]
    [AuthorizeAspect(RolesAuthorizationRequirement:"Admin")]
    public async Task<ReturnModel<CreateInstructorResponse>> CreateAsync(CreateInstructorRequest request)
    {
        bool anyInstructor = await instructorRepository.Where(x => x.Name == request.Name).AnyAsync();
        instructorBusinessRules.IsInstructorNameUnique(anyInstructor);

        Instructor instructor = mapper.Map<Instructor>(request);
        await instructorRepository.AddAsync(instructor);
        CreateInstructorResponse instructorAsDto = mapper.Map<CreateInstructorResponse>(instructor);

        return ReturnModel<CreateInstructorResponse>.Success(instructorAsDto,InstructorMessages.InstructorAddedMessage,HttpStatusCode.Created);

    }

    [ClearCacheAspect(cacheGroupKey: "GetInstructors")]
    [LoggerAspect]
    [AuthorizeAspect(RolesAuthorizationRequirement: "Admin")]
    public async Task<ReturnModel> DeleteAsync(Guid id)
    {
        Instructor? instructor = await instructorRepository.GetAsync(x=> x.Id == id);
        instructorBusinessRules.IsInstructorExists(instructor);

        await instructorRepository.DeleteAsync(instructor);

        return ReturnModel.Success(InstructorMessages.InstructorDeletedMessage,HttpStatusCode.NoContent);
    }

    [CacheAspect(cacheKeyTemplate:"GetInstructorsList",bypassCache:false,cacheGroupKey:"GetInstructors")]
    public async Task<ReturnModel<List<InstructorResponse>>> GetAllAsync()
    {
        List<Instructor> instructors = await instructorRepository.GetListAsync();

        List<InstructorResponse> instructorsAsDto = mapper.Map<List<InstructorResponse>>(instructors);

        return ReturnModel<List<InstructorResponse>>.Success(instructorsAsDto,InstructorMessages.InstructorsListedMessage);
       
    }

    public async Task<ReturnModel<InstructorResponse>> GetByIdAsync(Guid id)
    {
        Instructor? instructor = await instructorRepository.GetAsync(x=> x.Id == id);
        instructorBusinessRules.IsInstructorExists(instructor);

        InstructorResponse instructorAsDto = mapper.Map<InstructorResponse>(instructor);

        return ReturnModel<InstructorResponse>.Success(instructorAsDto,InstructorMessages.GetInstructorByIdMessage);
    }

    [ClearCacheAspect(cacheGroupKey: "GetInstructors")]
    [LoggerAspect]
    [ValidationAspect(typeof(UpdateInstructorRequestValidator))]
    [AuthorizeAspect(RolesAuthorizationRequirement: "Admin")]
    public async Task<ReturnModel<UpdateInstructorResponse>> UpdateAsync(UpdateInstructorRequest request)
    {
        Instructor instructor = await instructorRepository.GetAsync(x=> x.Id == request.Id);
        instructorBusinessRules.IsInstructorExists(instructor);

        bool anyInstructor = await instructorRepository.Where(x=> x.Name == request.Name && x.Id != instructor.Id).AnyAsync();
        instructorBusinessRules.IsInstructorNameUnique(anyInstructor);

        instructor = mapper.Map(request,instructor);

        await instructorRepository.UpdateAsync(instructor);

        var instructorAsDto = mapper.Map<UpdateInstructorResponse>(instructor);

        return ReturnModel<UpdateInstructorResponse>.Success(instructorAsDto,InstructorMessages.InstructorUpdatedMessage,HttpStatusCode.NoContent);

    }
}
