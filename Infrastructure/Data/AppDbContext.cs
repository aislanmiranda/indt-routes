using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    // dotnet ef migrations add StartProject --project ./Infrastructure/Infrastructure.csproj --startup-project ./Api/Api.csproj
    // dotnet ef database update --project ./Infrastructure/Infrastructure.csproj --startup-project ./Api/Api.csproj
    // dotnet ef migrations remove --force --project ./Infrastructure/Infrastructure.csproj --startup-project ./Api/Api.csproj

    public DbSet<Route> Routes => Set<Route>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RouteEntityConfig).Assembly);
      
        base.OnModelCreating(modelBuilder);
    }
}