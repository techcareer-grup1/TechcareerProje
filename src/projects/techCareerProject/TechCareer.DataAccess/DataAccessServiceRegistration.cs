using Core.Security.Repositories.Abstracts;
using Core.Security.Repositories.Concretes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechCareer.DataAccess.Contexts;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.DataAccess.Repositories.Concretes;

namespace TechCareer.DataAccess;

public static class DataAccessServiceRegistration
{
    public static IServiceCollection AddDataAccessServices(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddScoped<ICategoryRepository, CategoryRepository>();


        services.AddScoped<IUserOperationClaimRepository, UserOperationClaimRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IOperationClaimRepository,OperationClaimRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IInstructorRepository, InstructorRepository>();
        services.AddScoped<IVideoEduRepository, VideoEduRepository>();
        services.AddDbContext<BaseDbContext>(opt =>
        {
            opt.UseSqlServer(configuration.GetConnectionString("SqlConnection"));
        }, ServiceLifetime.Scoped);

        return services;
    }
}