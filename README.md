# Single Sign-on Api Server

This Api is used by [Single Sign-on UI](https://github.com/laredoza/SingleSignOnUI) to manage [SingleSignOnIdentityServer ( IdentityServer4 functionality )](https://github.com/laredoza/SingleSignOnIdentityServer).

## Features and Requirements

### Features

- Manage Clients;
- Manage Api Resources;
- Manage Identity Resources;
- Manage Users;
- Manage Roles;


![Preview](https://raw.githubusercontent.com/laredoza/SingleSignOnUI/master/SingleSignOn.gif)

### Requirements
- [Single Sign-on UI](https://github.com/laredoza/SingleSignOnUI)
- [SingleSignOnIdentityServer ( IdentityServer4 functionality )](https://github.com/laredoza/SingleSignOnIdentityServer)

### Database Migration & Seeding
- Postgres ( Run the SingleSignOn.Migrations.Postgres project )
- Microsoft Sql Server ( Run the SingleSignOn.Migrations.SqlServer project. Not tested yet )

## Database Selection

### Postgres

Postgres is the default database selected.

- Update DatabaseType to "Postgres" in the appsettings.json file.
- Update the defaultConnection to "Host=localhost;Database=SingleSignOn;Username=postgres;Password=password1;"
- Run Postgres Migration & Seeding 

### Microsoft Sql Server

This has not been tested yet.

- Update DatabaseType to "MsSql" in the appsettings.json file.
- Update the defaultConnection to "Server=.\\SQLEXPRESS;Database=SingleSignOn;Trusted_Connection=True;MultipleActiveResultSets=true"
- Run Microsoft Sql Server migration & seeding 