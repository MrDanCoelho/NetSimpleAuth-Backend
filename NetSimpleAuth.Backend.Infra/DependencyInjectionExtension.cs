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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NetPOC.Backend.Application.Services;
using NetPOC.Backend.Domain;
using NetPOC.Backend.Domain.Interfaces.IRepositories;
using NetPOC.Backend.Domain.Interfaces.IServices;
using NetPOC.Backend.Infra.Maps;
using NetPOC.Backend.Infra.Migrations;
using NetPOC.Backend.Infra.Repositories;
using Npgsql;
using Serilog;
using Serilog.Extensions.Logging;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NetPOC.Backend.Infra
{
    public static class DependencyInjectionExtension
    {
        /// <summary>
        /// Helper de configuração de injeção de dependências
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> contendo os serviços da aplicação</param>
        /// <param name="config"><see cref="IConfiguration"/> da aplicação</param>
        public static void ConfigureAllServices(this IServiceCollection services, IConfiguration config)
        {
            services.ConfigureSettings(config);
            services.ConfigureRepositories(config);
            services.ConfigureServices();
            services.ConfigureDistributedCache(config);
            services.ConfigureLogger();
            services.ConfigureSwagger();
        }

        
        /// <summary>
        /// <see cref="AppSettings"/> configuration Helper
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> of the application</param>
        /// <param name="config"><see cref="IConfiguration"/> of the application</param>
        private static void ConfigureSettings(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<AppSettings>(config);
            services.AddSingleton(x => x.GetRequiredService<IOptions<AppSettings>>().Value);
        }


        /// <summary>
        /// Helper de configuração de repositórios da aplicação
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> contendo os serviços da aplicação</param>
        /// <param name="config"></param>
        private static void ConfigureRepositories(this IServiceCollection services, IConfiguration config)
        {
            services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // Add SQLite support to FluentMigrator
                    .AddPostgres()
                    // Set the connection string
                    .WithGlobalConnectionString(config.GetConnectionString("DefaultConnection"))
                    // Define the assembly containing the migrations
                    .ScanIn(typeof(TestMigration).Assembly).For.Migrations())
                // Enable logging to console in the FluentMigrator way
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                // Build the service provider
                .BuildServiceProvider(false);
            
            FluentMapper.Initialize(c =>
            {
                c.AddMap(new LogMap());
                c.AddMap(new RefreshTokenMap());
                c.AddMap(new UserMap());
                c.ForDommel();
            });

            services.AddTransient<IDbConnection>(a 
                => new NpgsqlConnection(config.GetConnectionString("DefaultConnection")));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }

        /// <summary>
        /// Helper de configuração de serviços da aplicação
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> contendo os serviços da aplicação</param>
        private static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<IAccountService, AccountService>();
        }

        /// <summary>
        /// Helper de configuração de cache distribuído
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> contendo os serviços da aplicação</param>
        /// <param name="config"><see cref="IConfiguration"/> da aplicação</param>
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
        /// Helper de configuração de logs da applicação
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> contendo os serviços da aplicação</param>
        private static void ConfigureLogger(this IServiceCollection services)
        {
            var providers = new LoggerProviderCollection();
            
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.Providers(providers)
                .CreateLogger();
            
            services.AddSingleton<ILoggerFactory>(sc =>
            {
                var providerCollection = sc.GetService<LoggerProviderCollection>();
                var factory = new SerilogLoggerFactory(null, true, providerCollection);

                foreach (var provider in sc.GetServices<ILoggerProvider>())
                    factory.AddProvider(provider);

                return factory;
            });
        }
        
        /// <summary>
        /// Helper de configuração do Swagger
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> contendo os serviços da aplicação</param>
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
    
    public class RemoveVersionFromParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var versionParameter = operation.Parameters.Single(p => p.Name == "version");
            operation.Parameters.Remove(versionParameter);
        }
    }

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
}