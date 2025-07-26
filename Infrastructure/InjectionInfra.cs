using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjectionInfra
{
    public static void AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        var HOST = Environment.GetEnvironmentVariable("DBHOST");
        var PORT = Environment.GetEnvironmentVariable("DBPORT");
        var DB = Environment.GetEnvironmentVariable("DBNAME");
        var USER = Environment.GetEnvironmentVariable("DBUSER");
        var PASS = Environment.GetEnvironmentVariable("DBPASS");

        string connectionString = $"Host={HOST};Port={PORT};Database={DB};Username={USER};Password={PASS};Pooling=true;";

        #if DEBUG
        connectionString = configuration.GetConnectionString("DefaultConnection")!;
        #endif

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddTransient<IRouteRepository, RouteRepository>();        
    }
}

