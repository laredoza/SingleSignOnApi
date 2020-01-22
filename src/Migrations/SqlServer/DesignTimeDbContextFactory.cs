using System.IO;
using System.Reflection;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SingleSignOn.Migrations.SqlServer
{
    public class DesignTimeDbContextFactory<T> : IDesignTimeDbContextFactory<T>
        where T : DbContext
    {
        private ServiceProvider serviceProvider;
        private IServiceScope scope;

        public T CreateDbContext(string[] args)
        {
            var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

            IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var services = new ServiceCollection();
            services.AddTransient<IdentityServer4.EntityFramework.Options.ConfigurationStoreOptions>();
            services.AddTransient<IdentityServer4.EntityFramework.Options.OperationalStoreOptions>();

            var optionsBuilder = new DbContextOptionsBuilder<ConfigurationDbContext>();
            optionsBuilder.UseSqlServer(connectionString, a => a.MigrationsAssembly(migrationsAssembly)); 
            services.AddTransient<DbContextOptions<ConfigurationDbContext>>( p => optionsBuilder.Options);

            services.AddDbContext<T>(options =>
            {
                options.UseSqlServer(connectionString, a => a.MigrationsAssembly(migrationsAssembly));
            });

            serviceProvider = services.BuildServiceProvider();

            scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            return scope.ServiceProvider.GetService<T>();
        }

        public void Dispose()
        {
            if (this.serviceProvider != null)
            {
                this.serviceProvider.Dispose();
            }
            if (this.scope != null)
            {
                this.scope.Dispose();
            }
        }
    }
}