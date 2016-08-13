using System.IO;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Test.Models;

namespace Test
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment hosting)
        {
            // Setup configuration sources.
            var builder = new ConfigurationBuilder()
              .SetBasePath(hosting.ContentRootPath)
              .AddJsonFile("Config.json", optional: true, reloadOnChange: true)
              .AddJsonFile($"Config.{hosting.EnvironmentName}.json", optional: true)
              .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddOptions();
            services.Configure<ConfigModel>(Configuration.GetSection("Config"));
            services.AddSingleton<IHomeModel, HomeModel>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory log)
        {
            log.AddConsole(minLevel: LogLevel.Information);
            var logger = log.CreateLogger("test");

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            //I has been using it before I migrated project do dotnet CLI
            //app.UseIISPlatformHandler();
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
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
