using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NetPOC.Backend.Application.Services;
using NetPOC.Backend.Domain;
using NetPOC.Backend.Domain.Interfaces.IRepositories;
using NetPOC.Backend.Domain.Interfaces.IServices;
using NetPOC.Backend.Infra.Repositories;
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
            services.ConfigureRepositories();
            services.ConfigureServices();
            services.ConfigureDistributedCache(config);
            services.ConfigureLogger();
            services.ConfigureSwagger();
        }
        
        /// <summary>
        /// Helper de configuração de <see cref="AppSettings"/> da applicação
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> contendo os serviços da aplicação</param>
        /// <param name="config"><see cref="IConfiguration"/> da aplicação</param>
        private static void ConfigureSettings(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<AppSettings>(config);
            services.AddSingleton(x => x.GetRequiredService<IOptions<AppSettings>>().Value);
        }
        
        /// <summary>
        /// Helper de configuração de repositórios da aplicação
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> contendo os serviços da aplicação</param>
        private static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddDbContext<DataContext>(
                options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));
            
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        }

        /// <summary>
        /// Helper de configuração de serviços da aplicação
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> contendo os serviços da aplicação</param>
        private static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IUsuarioService, UsuarioService>();
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
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "NetPOC.Backend.API", Version = "v1"});
                
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
            foreach (var path in swaggerDoc.Paths)
            {
                paths.Add(path.Key.Replace("v{version}", swaggerDoc.Info.Version), path.Value);
            }

            swaggerDoc.Paths = paths;
        }
    }
}