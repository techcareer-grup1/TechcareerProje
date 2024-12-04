
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

