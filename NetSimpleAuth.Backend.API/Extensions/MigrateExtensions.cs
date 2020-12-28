using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace NetSimpleAuth.Backend.API.Extensions
{
    public static class MigrationExtension
    {
        public static IApplicationBuilder MigrateDatabase(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var runner = scope.ServiceProvider.GetService<IMigrationRunner>();
            
            if (runner == null) return app;
            
            runner.ListMigrations();
            runner.MigrateUp(1);

            return app;
        }
    }
}