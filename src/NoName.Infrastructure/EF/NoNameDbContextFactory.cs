using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace NoName.Infrastructure.EF
{
    public class NoNameDbContextFactory: IDesignTimeDbContextFactory<NoNameDbContext>
    {
        public NoNameDbContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString =
                configuration.GetConnectionString("NoNameDB");

            var optionsBuilder = new DbContextOptionsBuilder<NoNameDbContext>();
            optionsBuilder.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            });

            optionsBuilder.UseSqlServer(connectionString);

            return new NoNameDbContext (optionsBuilder.Options);
        }
    }
}