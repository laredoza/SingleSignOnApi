using SingleSignOn.Data.Context;
using SingleSignOn.Migrations.SqlServer;

namespace SingleSignOn.Migrations.Postgres
{
    public class ApplicationDesignTimeDbContextFactory : DesignTimeDbContextFactory<ApplicationDbContext>
    {
    }
}