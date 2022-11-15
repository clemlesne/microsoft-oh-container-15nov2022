﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using poi.Data;
using poi.Utility;
using Prometheus;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace poi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });

            var connectionString = poi.Utility.POIConfiguration.GetConnectionString(this.Configuration);
            services.AddDbContext<POIContext>(options =>
                options.UseSqlServer(connectionString));

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                // TODO: Build error "The type or namespace name 'Info' could not be found after migration from Swashbuckle v3 to v5"
                // c.SwaggerDoc("docs", new Info { Title = "Trip Insights Points Of Interest (POI) API", Description = "API for the trips in the Trip Insights app. https://github.com/clemlesne/microsoft-oh-container-15nov2022", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger)
        {
            // set up prometheus
            app.UseMetricServer();
            app.UseHttpMetrics();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }else{
                // https://github.com/prometheus-net/prometheus-net#aspnet-core-http-request-metrics
                // "You should use either UseExceptionHandler() or a custom exception handler middleware.
                // prometheus-net cannot see what the web host's default exception handler does and may report
                // the wrong HTTP status code for exceptions (e.g. 200 instead of 500)."
                app.UseExceptionHandler(a => a.Run(async context =>
                {
                    // https://stackoverflow.com/a/55166404/697126
                    // Should log but don't expose exception to user
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var exception = exceptionHandlerPathFeature.Error;

                    logger.LogError(exception, exception.Message);

                    var result = JsonConvert.SerializeObject(new { error = "An error occurred.  Please try again." });
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(result);
                }));
            }

            app.UseRewriter(new RewriteOptions().AddRedirect("(.*)api/docs/poi$", "$1api/docs/poi/index.html"));

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c =>
                c.RouteTemplate = "swagger/{documentName}/poi/swagger.json"
            );

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/docs/poi/swagger.json", "Trip Insights Points Of Interest (POI) API V1");
                c.DocumentTitle = "POI Swagger UI";
                c.RoutePrefix = "api/docs/poi";
            });

            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
