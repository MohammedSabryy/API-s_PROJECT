using E_Commerce.API.Middlewares;

namespace E_Commerce.API.Extensions
{ 
    public static class WepApplicationExtensions
    {
        public static async Task<WebApplication> SeedDbAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var DbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            await DbInitializer.InitilaizeAsync();
            await DbInitializer.InitilaizeIdentityAsync();
            return app;
        }

        public static WebApplication UseCustomExceptionMiddleare(this WebApplication app)
        {
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();
            return app;
        }
    }
}
