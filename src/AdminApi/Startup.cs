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
             // EntityFramework
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            
            services.AddDbContext<ApplicationDbContext>(options =>
                //options.UseSqlite(Configuration.GetConnectionString("SqlLite")));
                //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            // Manual Setup
            services.AddDbContext<ConfigurationDbContext>(options =>
                //options.UseSqlite(Configuration.GetConnectionString("SqlLite")));
                //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<IdentityServer4.EntityFramework.Options.ConfigurationStoreOptions>();


            services.AddDbContext<PersistedGrantDbContext>(options =>
                //options.UseSqlite(Configuration.GetConnectionString("SqlLite")));
                //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<IdentityServer4.EntityFramework.Options.OperationalStoreOptions>();

            services.AddMvcCore(options => options.EnableEndpointRouting = true)
                .AddAuthorization()
                .AddJsonFormatters();

            services.AddApiVersioning(
                options =>
                    {
                        // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                        options.ReportApiVersions = true;
                    });
            services.AddVersionedApiExplorer(
                options =>
                    {
                        // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                        // note: the specified format code will format the version as "'v'major[.minor][-status]"
                        options.GroupNameFormat = "'v'VVV";

                        // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                        // can also be used to control the format of the API version in route templates
                        options.SubstituteApiVersionInUrl = true;
                    });
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(
                options =>
                    {
                        // add a custom operation filter which sets default values
                        options.OperationFilter<SwaggerDefaultValues>();

                        // integrate xml comments
                        // options.IncludeXmlComments( XmlCommentsFilePath );
                        });

            var settings = Configuration.GetSection("Url").Get<UrlSettings>();

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = settings.Authority;
                    options.RequireHttpsMetadata = false;
                    options.Audience = "admin-api";
                });

            services.AddAuthorization(options =>
            {
                // options.AddPolicy("UserManagement", policy => policy.RequireClaim("scope", "admin_users"));
                options.AddPolicy("UserManagement", policy => policy.RequireClaim("admin_users", "true"));
                options.AddPolicy("RoleManagement", policy => policy.RequireClaim("admin_roles", "true"));
                options.AddPolicy("ApiManagement", policy => policy.RequireClaim("admin_api_resource", "true"));
                options.AddPolicy("IdentityManagement", policy => policy.RequireClaim("admin_identity_resource", "true"));
                options.AddPolicy("ClientManagement", policy => policy.RequireClaim("admin_clients", "true"));

                
                // options.AddPolicy("Consumer", policy => policy.RequireClaim(ClaimTypes.Role, "consumer"));
            });

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