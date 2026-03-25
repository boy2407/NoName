using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NoName.Application.Abstractions;
using NoName.Application.Abstractions.Persistence;
using NoName.Application.Abstractions.Services;
using NoName.Application.Common.Behaviors;
using NoName.Application.Services;
using System.Reflection;

namespace NoName.Application
{
    public static class DependencyInjection
    {


        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddValidatorsFromAssembly(typeof(IApplicationAssemblyMarker).Assembly);
            services.AddAutoMapper(typeof(IApplicationAssemblyMarker).Assembly);

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(IApplicationAssemblyMarker).Assembly);
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            });
            services.AddScoped<IMediaService, MediaService>();
            services.AddScoped<ILanguageService, LanguageService>();
            return services;
        }
    }
}