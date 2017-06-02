using System;
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
            mvcbuilder.AddJsonOptions(m => m.SerializerSettings.ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy () });
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
                app.UseExceptionHandler("/Home/Error");
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

            //app.UseMiddleware<RequestCultureMiddleware>();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
    //public class RequestCultureMiddleware
    //{
    //    private readonly RequestDelegate _next;

    //    public RequestCultureMiddleware(RequestDelegate next)
    //    {
    //        _next = next;
    //    }

    //    public Task Invoke(HttpContext context)
    //    {

    //        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
    //        context.Response.Headers.Add("Access-Control-Allow-Methods", "POST,GET,OPTIONS,PUT,DELETE");
    //        context.Response.Headers.Add("Access-Control-Allow-Headers", "Origin, No-Cache, X-Requested-With, If-Modified-Since, Pragma, Last-Modified, Cache-Control, Expires, Content-Type, X-E4M-With");
    //        return this._next(context);
    //    }
    //}
}
