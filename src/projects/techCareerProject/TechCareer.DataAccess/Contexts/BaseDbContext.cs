﻿using System.Reflection;
using Core.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TechCareer.DataAccess.Contexts;

public class BaseDbContext : DbContext
{
    public IConfiguration Configuration { get; set; }
    
    public BaseDbContext(DbContextOptions<BaseDbContext> opt, IConfiguration configuration) : base(opt)
    {
        Configuration = configuration;
  
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public DbSet<User> Users { get; set; }
    public DbSet<OperationClaim> OperationClaims { get; set; }
    public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
}