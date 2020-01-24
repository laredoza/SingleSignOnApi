// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

// using Swashbuckle.AspNetCore.SwaggerGen;

namespace SingleSignOn.AdminApi
{
    using System.Reflection;
    using System.Security.Claims;
    using global::AdminApi.V1.Services;
    using IdentityServer4.EntityFramework.DbContexts;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.AspNetCore.Rewrite;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;
    using SingleSignOn.Data.Context;
    using SingleSignOn.IdentityServerAspNetIdentity.Models;
    using SingleSignOn.Infrastructure.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using SingleSignOn.AdminApi.Extensions;

    public class Startup
    {

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Gets the environment.
        /// </summary>
        public IHostingEnvironment Environment { get; }

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationContext(this.Configuration);
            services.AddConfigurationContext(this.Configuration);
            services.AddPesistedContext(this.Configuration);

            services.AddMvcCore(options => options.EnableEndpointRouting = true)
                .AddAuthorization()
                .AddJsonFormatters();

            services.AddApiVersioningAndSwagger();
            var settings = Configuration.GetSection("Url").Get<UrlSettings>();
            services.AddAuthentictionAndAuthorisation(settings);

            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins(settings.CorsUrl)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            // Services
            services.AddTransient<IConfigurationManagementService, ConfigurationManagementService>();
            services.AddTransient<IIdentityManagerService, IdentityManagerService>();
        }

        public void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            // Default rewrite to swagger documentation.
            app.UseRewriter(new RewriteOptions()
                .AddRedirect("^$", "/swagger")
            );
            
            app.UseCors("default");
            app.UseAuthentication();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                    {
                        // build a swagger endpoint for each discovered API version
                        foreach (var description in provider.ApiVersionDescriptions)
                        {
                            options.SwaggerEndpoint(
                                $"/swagger/{description.GroupName}/swagger.json",
                                description.GroupName.ToUpperInvariant());
                        }
                    });
        }
    }
}