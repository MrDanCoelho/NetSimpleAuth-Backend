using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NetSimpleAuth.Backend.Application.Services;
using NetSimpleAuth.Backend.Domain;
using NetSimpleAuth.Backend.Domain.Interfaces.IRepositories;
using NetSimpleAuth.Backend.Domain.Interfaces.IServices;
using NetSimpleAuth.Backend.Infra.Maps;
using NetSimpleAuth.Backend.Infra.Migrations;
using NetSimpleAuth.Backend.Infra.Repositories;
using Npgsql;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NetSimpleAuth.Backend.Infra;

public static class DependencyInjectionExtension
{
    /// <summary>
    /// Dependency injection helper method
    /// </summary>
    /// <param name="services">The app's <see cref="IServiceCollection"/></param>
    /// <param name="config">The app's <see cref="IConfiguration"/></param>
    public static void ConfigureAllServices(this IServiceCollection services, IConfiguration config)
    {
        services.ConfigureSettings(config);
        services.ConfigureRepositories(config);
        services.ConfigureServices();
        services.ConfigureDistributedCache(config);
        services.ConfigureLogger(config);
        services.ConfigureSwagger();
    }

        
    /// <summary>
    /// <see cref="AppSettings"/> configuration Helper
    /// </summary>
    /// <param name="services">The app's <see cref="IServiceCollection"/></param>
    /// <param name="config">The app's <see cref="IConfiguration"/></param>
    private static void ConfigureSettings(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<AppSettings>(config);
        services.AddSingleton(x => x.GetRequiredService<IOptions<AppSettings>>().Value);
    }


    /// <summary>
    /// Repository configuration helper
    /// </summary>
    /// <param name="services">The app's <see cref="IServiceCollection"/></param>
    /// <param name="config">The app's <see cref="IConfiguration"/></param>
    private static void ConfigureRepositories(this IServiceCollection services, IConfiguration config)
    {
        services.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres()
                .WithGlobalConnectionString(config.GetConnectionString("DefaultConnection"))
                .ScanIn(typeof(TestMigration).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .BuildServiceProvider(false);
            
        FluentMapper.Initialize(c =>
        {
            c.AddMap(new LogMap());
            c.AddMap(new RefreshTokenMap());
            c.AddMap(new UserMap());
            c.ForDommel();
        });

        services.AddTransient<IDbConnection>(_ 
            => new NpgsqlConnection(config.GetConnectionString("DefaultConnection")));
        services.AddTransient<IUnitOfWork, UnitOfWork>();
            
        services.AddScoped<ILogRepository, LogRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
    }

    /// <summary>
    /// Service configuration helper
    /// </summary>
    /// <param name="services">The app's <see cref="IServiceCollection"/></param>
    private static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<ILogService, LogService>();
        services.AddScoped<IAccountService, AccountService>();
    }

    /// <summary>
    /// Distributed cache helper
    /// </summary>
    /// <param name="services">The app's <see cref="IServiceCollection"/></param>
    /// <param name="config">The app's <see cref="IConfiguration"/></param>
    private static void ConfigureDistributedCache(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetSection("ConnectionStrings:DistributedCache").Value;

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = connectionString;

            var assemblyName = Assembly.GetEntryAssembly()?.GetName();
            if (assemblyName != null) options.InstanceName = assemblyName.Name;
        });
    }

    /// <summary>
    /// Logging configuration helper
    /// </summary>
    /// <param name="services">The app's <see cref="IServiceCollection"/></param>
    /// <param name="config">The app's <see cref="IConfiguration"/></param>
    private static void ConfigureLogger(this IServiceCollection services, IConfiguration config)
    {
        var serilogLogger = new LoggerConfiguration()
            .ReadFrom.Configuration(config)
            .CreateLogger();
 
        services.AddLogging(builder =>
        {
            builder.AddSerilog(logger: serilogLogger, dispose: true);
        });
    }
        
    /// <summary>
    /// Swagger configuration helper
    /// </summary>
    /// <param name="services">The app's <see cref="IServiceCollection"/></param>
    private static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo {Title = "NetSimpleAuth.Backend.API", Version = "v1"});
                
            c.OperationFilter<RemoveVersionFromParameter>();
                
            c.DocumentFilter<ReplaceVersionWithExactValueInPath>();

            c.DocInclusionPredicate((version, desc) =>
            {
                var actionDescriptor = desc.ActionDescriptor.DisplayName;

                if (actionDescriptor == null) return false;
                if (!actionDescriptor.Contains(version)) return false;

                if (desc.RelativePath == null) return true;
                
                var values = desc.RelativePath.Split("/").Select(v => v.Replace("v{version}", version));

                desc.RelativePath = string.Join("/", values);

                return true;
            });
                
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                In = ParameterLocation.Header,
                Description = "JWT Bearer token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT"
            });
                
            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference()
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
                
            var xmlFile = $"{Assembly.GetEntryAssembly()?.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            
            c.IncludeXmlComments(xmlPath);
        });
    }
}
    
// ReSharper disable once ClassNeverInstantiated.Global
public class RemoveVersionFromParameter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var versionParameter = operation.Parameters.Single(p => p.Name == "version");
        operation.Parameters.Remove(versionParameter);
    }
}

// ReSharper disable once ClassNeverInstantiated.Global
public class ReplaceVersionWithExactValueInPath : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var paths = new OpenApiPaths();
        foreach (var (key, value) in swaggerDoc.Paths)
        {
            paths.Add(key.Replace("v{version}", swaggerDoc.Info.Version), value);
        }

        swaggerDoc.Paths = paths;
    }
}