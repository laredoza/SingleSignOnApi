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
- Update the defaultConnection to "Host=localhost;Database=SingleSignOn;Username=postgres;Password=password1;" in the appsettings.json
- Run Postgres Migration & Seeding 

### Microsoft Sql Server

This has not been tested yet.

- Update DatabaseType to "MsSql" in the appsettings.json file.
- Update the defaultConnection to "Server=.\\SQLEXPRESS;Database=SingleSignOn;Trusted_Connection=True;MultipleActiveResultSets=true" in the appsettings.json
- Run Microsoft Sql Server Migration & Seeding 

## Database Installation

### Postgres

```
docker stop pg-docker 
docker rm pg-docker 
docker run \
	--name pg-docker \
	-e POSTGRES_PASSWORD=password1 \
	-d --restart unless-stopped \
	-p 5432:5432 \
	-v /home/docker/postgress/data:/var/lib/postgresql/data \
	postgres
```