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
using Microsoft.AspNetCore.Mvc;
using E_Commerce.API.Factories;
using E_Commerce.API.Extensions;

namespace E_Commerce.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            #region Services
            builder.Services.AddCoreServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddPresentationServices();
            #endregion

            var app = builder.Build();

            #region PipeLines
            
            await app.SeedDbAsync();
            app.UseCustomExceptionMiddleare();
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
            #endregion
            
        }

    }
}
