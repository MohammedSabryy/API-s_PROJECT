global using Services.Abstractions;
global using Services;
global using E_Commerce.API.Factories;
global using Microsoft.AspNetCore.Mvc;
global using Persistence.Repositories;
global using StackExchange.Redis;
global using Persistence.Identity;
global using Microsoft.AspNetCore.Identity;
global using Domain.Entities;
global using Microsoft.IdentityModel.Tokens;
using Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

namespace E_Commerce.API.Extensions
{
    public static class InfraStructureServicesExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBasketRepository, BasketRepository>();

            services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultSQLConnection"));
            });

            services.AddDbContext<StoreIdentityContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentitySQLConnection"));
            });

            services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer
            .Connect(configuration.GetConnectionString("Redis")!));
            services.ConfigureIdentityService();
            services.ConfigureJwt(configuration);
            return services;
        }

        public static IServiceCollection ConfigureIdentityService(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(options=>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<StoreIdentityContext>();
            return services;
        }

        public static IServiceCollection ConfigureJwt(this IServiceCollection services , IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection("jwtOptions").Get<JwtOptions>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options=>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtOptions.Issure,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                };

            });
            services.AddAuthorization();
            return services;
        }
    }

     
}
