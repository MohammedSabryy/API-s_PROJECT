using E_Commerce.API.Factories;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repositories;
 using Microsoft.OpenApi.Models;

namespace E_Commerce.API.Extensions
{
    public static class PresentationServicesExtensions
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            services.AddControllers().AddApplicationPart(typeof(Presentation.AssembelyReference).Assembly);
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ApiResponseFactory.CustomValidationErrorResponse;
            });
            services.ConfigureSwagger();


            return services;
        }
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please Enter Bearer Token.",
                    Name = "Authourization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new List<string>(){ }
                    }
                }); 
            });
            return services;
        }
    }
}
