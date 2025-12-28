using Academic.Application.Interfaces;
using Academic.Domain.Interfaces;
using Academic.Infrastructure.Data;
using Academic.Infrastructure.Persistence.Repositories;
using Academic.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace Academic.Infrastructure
{
    public static class DependencyInjection
    {
      
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // 1. Register the Application DbContext
            services.AddDbContext<AcademicDbContext>(options =>
            {
                
                options.UseSqlServer(configuration.GetConnectionString("AcademicServiceDbConnection"));
            });


            // 2. Register Repositories (Applying Dependency Inversion Principle - DIP)

            
            
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IClassRepository, ClassRepository>();
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            services.AddScoped<IClassSubjectRepository, ClassSubjectRepository>();



            return services;
        }
    }
}