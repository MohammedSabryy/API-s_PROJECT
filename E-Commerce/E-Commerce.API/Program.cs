global using Domain.Contracts;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Options;
global using Persistence;
global using Persistence.Data;
global using AutoMapper;
using Services.Abstractions;
using Services;
using Persistence.Repositories;
using E_Commerce.API.Middlewares;

namespace E_Commerce.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddApplicationPart(typeof(Presentation.AssembelyReference).Assembly);
            builder.Services.AddScoped<IDbInitializer, DbInitializer>();
            builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
            builder.Services.AddScoped<IServiceManager, ServiceManager>();


            builder.Services.AddAutoMapper(typeof(Services.AssembleyReference).Assembly);
            builder.Services.AddDbContext<StoreContext>(options=> 
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseMiddleware<GlobalErrorHandlingMiddleware>();
            await InitializeDbAsync(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
            async Task InitializeDbAsync(WebApplication app)
            {
                using var scope = app.Services.CreateScope();
                var DbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                await DbInitializer.InitilaizeAsync();
            }
        }

    }
}
