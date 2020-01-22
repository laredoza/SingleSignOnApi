using System;
using System.IO;
using System.Reflection;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SingleSignOn.Data.Context;
using SingleSignOn.Data.Seeding;
using SingleSignOn.IdentityServerAspNetIdentity.Models;

namespace SingleSignOn.Migrations.Postgres
{
    class Program
    {
        static void Main(string[] args)
        {
            var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
            var services = new ServiceCollection();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    // .AddEnvironmentVariables()
                    .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var serverUrl = configuration["serverUrl"];
            
            services.AddDbContext<ApplicationDbContext>(
                options =>

                    // options.UseSqlite(connectionString));
                    // options.UseSqlServer(connectionString));
                    options.UseNpgsql(connectionString, a => a.MigrationsAssembly(migrationsAssembly)));

            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            
            // Manual Setup
            services.AddDbContext<ConfigurationDbContext>(options =>
                //options.UseSqlite(Configuration.GetConnectionString("SqlLite")));
                //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
                options.UseNpgsql(connectionString, a => a.MigrationsAssembly(migrationsAssembly)));
            services.AddTransient<IdentityServer4.EntityFramework.Options.ConfigurationStoreOptions>();


            services.AddDbContext<PersistedGrantDbContext>(options =>
                //options.UseSqlite(Configuration.GetConnectionString("SqlLite")));
                //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
                options.UseNpgsql(connectionString, a => a.MigrationsAssembly(migrationsAssembly)));
            services.AddTransient<IdentityServer4.EntityFramework.Options.OperationalStoreOptions>();
            
            services.AddTransient<ISeedData, SeedData>();

            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    ISeedData seedData = scope.ServiceProvider.GetService<ISeedData>();
                    seedData.SeedIdentity();
                    seedData.SeedIdentityServer(serverUrl);
                }
            }
        }
    }
}
