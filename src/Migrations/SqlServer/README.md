# Executing Migrations and Seeding Data

- Update connection string in appsettings.json
- Run application SingleSignOn.Migrations.Postgres.exe 

## Update Migrations 

- Go to correct project 
cd src/Migrations/SqlServer
- Save Application Migration (if required)
dotnet ef migrations add --context SingleSignOn.Data.Context.ApplicationDbContext 'Initial' -o Migrations/Application
- Save Configuration Migration (if required)
dotnet ef migrations add --context IdentityServer4.EntityFramework.DbContexts.ConfigurationDbContext 'Initial' -o ./Migrations/Config
- Save PersistedGrantDbContext Migration (if required)
dotnet ef migrations add --context IdentityServer4.EntityFramework.DbContexts.PersistedGrantDbContext 'Initial' -o ./Migrations/PersistedGrant

## Update Migration Scripts

- Save Application Migration Script (if required)
dotnet ef migrations script --context SingleSignOn.Data.Context.ApplicationDbContext -o Migrations/Application/Scripts/migration.sql
- Save Configuration Migration Script (if required)
dotnet ef migrations script --context IdentityServer4.EntityFramework.DbContexts.ConfigurationDbContext -o ./Migrations/Config/Scripts/migration.sql
- Save PersistedGrantDbContext Migration Script (if required)
dotnet ef migrations script --context IdentityServer4.EntityFramework.DbContexts.PersistedGrantDbContext -o ./Migrations/PersistedGrant/Scripts/migration.sql

## Manually Update Database
- dotnet ef database update