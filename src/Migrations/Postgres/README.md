# Executing Migrations and Seeding Data

- Update connection string in appsettings.json
- Run application SingleSignOn.Migrations.Postgres.exe 

# Update Migrations 

- Go to correct project 
cd src/Migrations/Postgres/
- Save Application Migration (if required)
dotnet ef migrations add --context SingleSignOn.Data.Context.ApplicationDbContext 'Initial' -o Migrations/Application
- Save Configuration Migration (if required)
dotnet ef migrations add --context IdentityServer4.EntityFramework.DbContexts.ConfigurationDbContext 'Initial' -o ./Migrations/Config
<!-- dotnet ef migrations add --context SingleSignOn.Data.Context.ConfigDbContext 'Initial' -o ./Migrations/Config -->
- Save PersistedGrantDbContext Migration (if required)
dotnet ef migrations add --context IdentityServer4.EntityFramework.DbContexts.PersistedGrantDbContext 'Initial' -o ./Migrations/PersistedGrant
- Manually Update Database
dotnet ef database update