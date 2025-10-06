using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebAPI.Domain.Interfaces;
using WebAPI.Infrastructure.Data;
using WebAPI.Infrastructure.Repositories;
using WebAPI.Infrastructure.Interceptors;

namespace WebAPI.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Register Current User Service
        services.AddScoped<ICurrentUserService, DefaultCurrentUserService>();

        // Register Interceptors
        services.AddScoped<AuditInterceptor>();

        // Register DbContext with interceptors
        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
            .AddInterceptors(serviceProvider.GetRequiredService<AuditInterceptor>()));

        // Register Unit of Work
        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<ApplicationDbContext>());

        // Register Repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        return services;
    }
}