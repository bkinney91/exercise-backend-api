using System;
using System.Collections.Generic;
using AspNetCoreRateLimit;
using FieldLevel.DataProviders;
using FieldLevel.DataProviders.Interfaces;
using FieldLevel.Services;
using FieldLevel.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;

namespace FieldLevel
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        public IConfiguration Configuration { get; }
        private IWebHostEnvironment WebHostEnvironment;
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Create and register logger
            Log.Logger = new LoggerConfiguration()
                                .WriteTo.Console()
                                .WriteTo.File("Logs/FieldLevelApi-.txt", rollingInterval: RollingInterval.Day)
                                .CreateLogger();

            //Configuration for ASPNETCORERateLimit
            services.AddOptions();
            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimit"));
            // inject counter and rules stores
            services.AddInMemoryRateLimiting();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddHttpContextAccessor();

            //Dependency Injection for the domain service and provider
            services.AddScoped<IPostService, PostService>();
            //If production use the live data source
            if(WebHostEnvironment.IsProduction())
            {
                Log.Information("Using environment \"Production\"");
                services.AddScoped<IPostProvider, PostApiProvider>();
                //Http Client
                services.AddHttpClient("postClient", c =>
                {
                    c.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/posts");
                });
            }
            else //all other environments use static posts.json file
            {
                Log.Information("Using environment \"Development\"");
                services.AddScoped<IPostProvider, PostFileProvider>();
            }           
          
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FieldLevel", Version = "v1" });
            });
          
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
            }
            app.UseIpRateLimiting();

            app.UseSwagger();
            
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FieldLevel v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
