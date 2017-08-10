using System;
using System.Diagnostics;
using System.Net.Http;
using AltInnSrr.Lib;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AltInnSrr.Api
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

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddOptions();
            services.Configure<AltInnEnvironment>(Configuration.GetSection("AltInnSrr"));
            services.Configure<AltInnEnvironment>(Configuration);

            services.AddMvc();
            services.AddTransient<ISrrClient, SrrClient>();
            services.AddTransient<IServiceClient, ServcieClient>();
            services.AddTransient<IEnhetsregisteretClient, EnhetsregisteretClient>();
            services.AddTransient<HttpClient>();
        }

        private void ConfigureOptions(AltInnEnvironment altInnEnvironment)
        {
            throw new NotImplementedException();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            Debug.WriteLine($"Miljø: {env.EnvironmentName}");
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
