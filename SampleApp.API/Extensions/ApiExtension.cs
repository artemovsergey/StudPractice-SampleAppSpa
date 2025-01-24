using SampleApp.API.Data;

namespace SampleApp.API.Extensions
{
    public static class ApiExtension
    {

        public static async Task ResetDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            try
            {
                var context = scope.ServiceProvider.GetService<SampleAppContext>();
                if (context != null)
                {
                    await context.Database.EnsureDeletedAsync();
                    await context.Database.EnsureCreatedAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Sql Exception:{ex.InnerException!.Message}");
            }
        }
    }
}