using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using AutoMapper;
using EnterpriseApp.API.Authorization;
using EnterpriseApp.API.Core.Authorizations;
using EnterpriseApp.API.Core.Mapping;
using EnterpriseApp.API.Core.Services;
using EnterpriseApp.API.Data;
using EnterpriseApp.API.Filters;
using EnterpriseApp.API.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace EnterpriseApp.API
{
    /// <summary>
    ///  
    /// </summary>
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddScoped<ModelValidationFilterAttribute>();

            services.AddControllers(config =>
            {

            });

            services.AddMvcCore()
            .AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.Converters.Add(new StringEnumConverter());
                o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            })
            .AddDataAnnotations()
            .AddApiExplorer();

            ConfigureAuthentication(services);

            ConfigureAuthorization(services);

            ConfigureCoreServices(services);

            ConfigureAutoMapper(services);

            ConfigureCors(services);
        }

        private void ConfigureAuthorization(IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, PermissionHandler>();

            services.AddAuthorization(options =>
                    {
                        List<string> permissionTypes = Permissions.GetAll();

                        foreach (var permissionType in permissionTypes)
                        {
                            options.AddPolicy(permissionType, policy =>
                        policy.Requirements.Add(new RoleClaimsAuthorizationRequirement("Permissions", new string[] { permissionType })));
                        }

                    });
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

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
                    ValidateIssuerSigningKey = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidIssuer = appSettings.Issuer,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        private void ConfigureCors(IServiceCollection services)
        {
            string[] corsOrigins = GetCorsOrigins();

            services.AddCors(options =>
            {
                options.AddPolicy("Default",
                builder => builder
                  .WithOrigins(corsOrigins)
                  .AllowAnyMethod()
                  .AllowAnyHeader());
            });
        }

        private string[] GetCorsOrigins()
        {
            List<string> corsSettings = Configuration.GetSection("Cors:Origins").Get<List<string>>();

            return corsSettings.ToArray();
        }

        public void ConfigureAutoMapper(IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new DomainDataMapperProfile());
                }
            );

            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);
            services.AddAutoMapper(typeof(Startup));
        }

        private void ConfigureCoreServices(IServiceCollection services)
        {
            //database
            services.AddScoped<IRepository, MongoDBRepository>();
            //services
            services.AddScoped<IApplicationDataService, ApplicationDataService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IEmailService, EmailService>();
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "EnterpriseApp API",
                    Version = "v1"
                });

                var xmlPath = System.AppDomain.CurrentDomain.BaseDirectory + @"EnterpriseApp.API.xml";
                s.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandlerMiddleware();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseCors("Default");

            InsertData();
        }

        private void InsertData()
        {
            try
            {

            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
