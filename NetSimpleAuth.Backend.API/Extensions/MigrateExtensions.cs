using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace NetSimpleAuth.Backend.API.Extensions
{
    /// <summary>
    /// Migration Extension helper
    /// </summary>
    public static class MigrationExtension
    {
        /// <summary>
        /// Migrates database on startup
        /// </summary>
        /// <param name="app">The current app</param>
        /// <returns>The application with migrated database</returns>
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