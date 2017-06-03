﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApp.Data;
using WebApp.Models;
using WebApp.Services;
using Newtonsoft.Json.Serialization;
using Core.Repository;
using Microsoft.AspNetCore.Http;

namespace WebApp
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var mvcbuilder= services.AddMvc();
            //mvcbuilder.AddJsonOptions(m => m.SerializerSettings.ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy () });
            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            
            
            services.AddMemoryCache();

            services.AddDistributedMemoryCache();
            services.AddDbContext<ApplicationDbContext>(options =>
                options
                .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), m => m.UseRowNumberForPaging())

                ).AddUnitOfWork<ApplicationDbContext>();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
                app.UseExceptionHandler(new ExceptionHandlerOptions { ExceptionHandlingPath= "/Home/Error", ExceptionHandler=ErrorRequest } );
            }
            app.UseCors(m=> {
                m.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();//.AllowCredentials();
            });
            app.UseStaticFiles();
            
            app.UseIdentity(); 

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715
            var dbcontext = app.ApplicationServices.GetService<ApplicationDbContext>();
            if (dbcontext.Database.EnsureCreated())
            {
                dbcontext.Database.MigrateAsync();
            }

            //app.UseMiddleware<RequestErrorMiddleware>();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public Task ErrorRequest(HttpContext context)
        {
            return Task.Factory.StartNew(delegate {
                if (context.Response.StatusCode != 500)
                {
                    var log = context.RequestServices.GetService<ILogger>();
                    log.LogInformation("Error");
                }
            });
        }
    }
    public class RequestErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            if (context.Response.StatusCode != 500)
            {
                //context.Response.Clear();
                var log= context.RequestServices.GetService<ILogger>();
                log.LogInformation("Error");
            }
            return this._next(context);
        }
    }
}
