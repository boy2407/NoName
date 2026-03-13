using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NoName.Application.Abstractions.Persistence;
using NoName.Infrastructure.EF;
using NoName.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Infrastructure
{
    public static class InfrastructureRegistration
    {
        //public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        //{
        //    //services.AddDbContext<NoNameDbContext>(options =>
        //    //    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        //    //services.AddScoped<ICategoryRepository, CategoryRepository>();
        //    //// Register other repositories and services here 

        //    //return services;
        //}
    }
}
