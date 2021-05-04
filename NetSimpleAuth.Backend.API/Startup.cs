using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using NetSimpleAuth.Backend.API.Extensions;
using NetSimpleAuth.Backend.Infra;

namespace NetSimpleAuth.Backend.API
{
    /// <summary>
    /// Starts the app
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Service Startup
        /// </summary>
        /// <param name="configuration">The <see cref="IConfiguration"/> of the app</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        /// <summary>
        /// Service injection method
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> of the app</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddApiVersioning(o =>
            {
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
            });
            
            services.ConfigureAllServices(Configuration);
            
            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("Secret").Value);
            
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }

        /// <summary>
        /// Service configuration method
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> of the app</param>
        /// <param name="env">The <see cref="IWebHostEnvironment"/> of the app</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(a =>
                {
                    a.AllowAnyHeader();
                    a.AllowAnyMethod();
                    a.AllowAnyOrigin();
                });
            }
            
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NetSimpleAuth.Backend.API v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.MigrateDatabase();
        }
    }
}