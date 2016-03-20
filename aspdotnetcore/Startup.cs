using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Test.Models;
using Microsoft.Extensions.Configuration;

namespace Test
{
    public class Startup
    {
        private IHostingEnvironment _hosting;

        public Startup(IHostingEnvironment hosting)
        {
            _hosting = hosting;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSingleton<IHomeModel, HomeModel>();

            var root =
              new ConfigurationBuilder().
              AddJsonFile("Config.json").
              Build();

            //???
            //services.AddOptions(); 
            services.Configure<ConfigModel>(root);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory log)
        {
            log.AddConsole(minLevel: LogLevel.Information);
            var logger = log.CreateLogger("test");

            if (_hosting.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseIISPlatformHandler();
            app.UseMvcWithDefaultRoute();
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "Home",
            //        template: "{controller}/{action}/{id}",
            //        defaults: new
            //        {
            //            controller = "Home",
            //            action = "Index"
            //        });
            //});
            app.UseStaticFiles();
            app.UseStatusCodePages();
            app.UseWelcomePage();

            //app.Use(async (context, next) =>
            //{
            //    logger.LogInformation("Before - First Hello World!");

            //    await context.Response.WriteAsync("First Hello World!");
            //    await next();

            //    logger.LogInformation("After - First Hello World!");
            //});

            //app.Run(async (context) =>
            //{
            //    logger.LogInformation("Before - Second Hello World!");

            //    await context.Response.WriteAsync("Second Hello World!");

            //    logger.LogInformation("Second - Second Hello World!");
            //});
        }

        // Entry point for the application.
        public static void Main(string[] args)
        {
            WebApplication.Run<Startup>(args);
        }
    }
}
