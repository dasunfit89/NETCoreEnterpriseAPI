using AutoMapper;
using EnterpriseApp.API.Core.Mapping;
using EnterpriseApp.API.Core.Services;
using EnterpriseApp.API.Core.Services.Interface;
using EnterpriseApp.API.Data;
using EnterpriseApp.API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace EnterpriseApp.BackOffice
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
               .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
           .AddJsonFormatters()
           .AddDataAnnotations()
           .AddApiExplorer();
             
            ConfigureCoreServices(services);

            ConfigureAutoMapper(services);
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            //ConfigureCors(services);
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
            services.AddAutoMapper();
        }

        private void ConfigureCoreServices(IServiceCollection services)
        {
            //database
            services.AddDbContext<EnterpriseAppContext>(options => options.UseMySQL(Configuration.GetConnectionString("DefaultConnection")));
            //services
            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFileService, FileService>();

            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(3);
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Auth}/{action=Login}/{id?}");
            });
        }
    }
}
