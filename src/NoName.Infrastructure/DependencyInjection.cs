using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.StackExchangeRedis;
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
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;
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
            //Database
            services.AddDbContext<NoNameDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("NoNameDB")));
            //options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));


            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.Configure<MomoSettings>(configuration.GetSection("PaymentSettings:Momo"));
            // Redis Configuration
            //var redisConnectionString = configuration.GetConnectionString("Redis") ;
            //var multiplexer = ConnectionMultiplexer.Connect(redisConnectionString);



            //var redisConnectionString = configuration.GetConnectionString("Redis");
            var redisConnectionString = "localhost:6379"; 

            services.AddSingleton<IConnectionMultiplexer>(sp =>
                ConnectionMultiplexer.Connect(redisConnectionString));
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString; 
                options.InstanceName = "NoName-";
            });


            //RedLock
            services.AddSingleton<IDistributedLockService, DistributedLockService>();
            services.AddSingleton<IDistributedLockFactory>(sp =>
            {
                var connection = sp.GetRequiredService<IConnectionMultiplexer>();

                var endpoints = connection.GetEndPoints().Select(e => new RedLockEndPoint(e)).ToList();


                return RedLockFactory.Create(endpoints);
            });



            services.AddKeyedScoped<IAIService, SemanticKernelService>(SemanticKernelKey);
           


            services.AddIdentity<User, Domain.Entities.Role>(options =>
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
            services.AddScoped<ICacheService, RedisCacheService>();
            // Register TokenService
            services.AddTransient<IEmailService, EmailService>();
            services.AddScoped<ITokenService, TokenService>();
            //  Repositories and UnitOfWork
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ILanguageRepository, LanguageRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Payment Services - Support Multiple Providers
            services.AddHttpClient();
            services.AddScoped<IPaymentService, MomoPaymentService>();
            // Add more payment providers here in the future (VNPay, ZaloPay, etc.)
            // services.AddScoped<IPaymentService, VNPayPaymentService>();


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
