# Single Sign-on Server

An Angular front end for IdentityServer4. [IdentityServer4](http://docs.identityserver.io/en/latest/) is an OpenID Connect and OAuth 2.0 framework for ASP.NET Core. 

### Features

- Manage Clients;
- Manage Api Resources;
- Manage Identity Resources;
- Manage Users;
- Manage Roles;

### There are two Api projects.
- AdminApi ( Dotnet core api used to manage the Idenity4 server ) 

### Database Migration & Seeding
- Postgres ( Run the SingleSignOn.Migrations.Postgres project )
- SqlServer ( Run the SingleSignOn.Migrations.SqlServer project. Not tested yet )

### There is also an angular admin application
- [Admin](https://dev.azure.com/laredoza/SingleSignOn) ( Angular application used to manage IdentityServer4 )

