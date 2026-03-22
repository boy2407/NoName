using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NoName.Application.Abstractions;
using NoName.Application.Abstractions.Persistence;
using NoName.Application.Abstractions.Services;
using NoName.Application.Contracts;
using NoName.Application.Services;
using NoName.Domain.Entities;
using NoName.Infrastructure.EF;
using NoName.Infrastructure.Persistence;
using NoName.Infrastructure.Services;
using NoName.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Infrastructure
{
    public static class DependencyInjection
    {
        public const string OllamaKey = "Ollama";
        public const string SemanticKernelKey = "SemanticKernel";
        public const string OpenRouterKey = "OpenRouter";
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<NoNameDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("NoNameDB")));

            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddKeyedScoped<IAIService, SemanticKernelService>(SemanticKernelKey);

            services.AddKeyedScoped<IAIService, OpenRouterFreeService>(OpenRouterKey, (sp, key) =>
            {
                var apiKey = configuration["AI:OpenRouter:ApiKey"];

                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    throw new InvalidOperationException("Missing configuration: AI:OpenRouter:ApiKey");
                }

                return new OpenRouterFreeService(apiKey);
            });


            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<NoNameDbContext>()
            .AddDefaultTokenProviders();


            services.AddHttpContextAccessor();

            // Register TokenService
            services.AddTransient<IEmailService, EmailService>();
            services.AddScoped<ITokenService, TokenService>();
            //  Repositories and UnitOfWork
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ILanguageRepository, LanguageRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();



            services.AddAuthorization(options =>
            {
               
                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireRole("Admin"));

                options.AddPolicy("ManagementContent", policy =>
                    policy.RequireRole("Admin", "Manager"));

                options.AddPolicy("VerifiedUser", policy =>
                    policy.RequireClaim("EmailConfirmed", "true"));

                //options.AddPolicy("HCMAdminOnly", policy =>
                //    policy.RequireRole("Admin").RequireClaim("City", "HCM"));
            });

            return services;
        }
    }
}
