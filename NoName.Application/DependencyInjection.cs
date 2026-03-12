using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;
using MediatR;
using NoName.Application.Common.Behaviors; 

namespace NoName.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // import Validators on the current assembly
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMediatR(cfg => {
                // Register Handler 
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

                //Connect ValidationBehavior with Command/Query
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });

            return services;
        }
    }
}