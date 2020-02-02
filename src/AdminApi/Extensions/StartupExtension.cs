// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StartupExtension" company="">
//
// </copyright>
// <summary>
//   The class StartupExtension.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SingleSignOn.AdminApi.Extensions
{
    #region Usings

    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using SingleSignOn.Data.Context;
    using Microsoft.Extensions.Configuration;
    using IdentityServer4.EntityFramework.DbContexts;
    using Microsoft.Extensions.Options;
    using SingleSignOn.Infrastructure.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;

    #endregion

    /// <summary>
    ///     The StartupExtension.
    /// </summary>
    public static class StartupExtension
    {
        #region Constructors and Destructors
        #endregion

        #region Public Properties


        #endregion

        #region Public Methods And Operators

        public static void AddApplicationContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                ConfigureDatabaseOptions(configuration, options);
            });
        }

        public static void AddConfigurationContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ConfigurationDbContext>(options =>
            {
                ConfigureDatabaseOptions(configuration, options);
            });

            services.AddTransient<IdentityServer4.EntityFramework.Options.ConfigurationStoreOptions>();
        }
        public static void AddPesistedContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PersistedGrantDbContext>(options =>
                {
                    ConfigureDatabaseOptions(configuration, options);
                });
            services.AddTransient<IdentityServer4.EntityFramework.Options.OperationalStoreOptions>();
        }

        public static void AddApiVersioningAndSwagger(this IServiceCollection services)
        {
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

        }

        public static void AddAuthentictionAndAuthorisation(this IServiceCollection services, UrlSettings settings)
        {
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
        }

        #endregion

        #region Other Methods

        private static void ConfigureDatabaseOptions(IConfiguration configuration, DbContextOptionsBuilder options)
        {
            var databaseType = (DatabaseType)Enum.Parse(typeof(DatabaseType), configuration.GetValue<string>("DatabaseType"));
            switch (databaseType)
            {
                case DatabaseType.Postgres:
                    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                    break;
                case DatabaseType.MsSql:
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                    break;
                case DatabaseType.MySql:
                    options.UseMySql(configuration.GetConnectionString("DefaultConnection"));
                    break;
                default:
                    throw new Exception("Database not implemented");
            }
        }

        #endregion
    }
}