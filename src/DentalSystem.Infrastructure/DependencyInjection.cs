using DentalSystem.Application.Ports.Persistence;
using DentalSystem.Application.Ports.Repositories;
using DentalSystem.Infrastructure.Persistence;
using DentalSystem.Infrastructure.Persistence.Repositories.Specialties;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DentalSystem.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration configuration)
        {
            // Add the database context
            services.AddDbContext<DentalSystemDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"))
            );


            // Add interfaces for repositories and unit of work
            // scoped: One instance per HTTP request
            services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // return services
            return services;
        }
    }
}
