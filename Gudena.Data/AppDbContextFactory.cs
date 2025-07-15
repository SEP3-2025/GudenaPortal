using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Gudena.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(
            "Host=bdx9ftw4a5fc6ez7vubi-postgresql.services.clever-cloud.com;Port=50013;Database=bdx9ftw4a5fc6ez7vubi;Username=uxes4wggvxt1lr0q7ft8;Password=t269iZnsBu8v6ArOUSIh6z0LwiG5vY"
        );

        return new AppDbContext(optionsBuilder.Options);
    }
}