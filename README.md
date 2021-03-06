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
- Postgres ( Run the SingleSignOn.Migrations.Postgres project or execute the migration scripts)
- Microsoft Sql Server ( Run the SingleSignOn.Migrations.SqlServer project or execute the migration scripts )
- MySql ( Run the SingleSignOn.Migrations.MySql project or execute the migration scripts )

## Database Configuration

### Postgres

Postgres is the default database selected.

- Update DatabaseType to "Postgres" in appsettings.
- Update the defaultConnection to "Host=localhost;Database=SingleSignOn;Username=postgres;Password=password1;" in  appsettings.json
- Run Postgres Migration & Seeding 

### Microsoft Sql Server

- Update DatabaseType to "MsSql" in appsettings.
- Update the defaultConnection to "Data Source=.;Initial Catalog=SingleSignOn;User ID=sa;Password=yourStrong(!)Password;" in appsettings.json
- Run Microsoft Sql Server Migration & Seeding 

### MySql

- Update DatabaseType to "MySql" in appsettings.
- Update the defaultConnection to "Server=localhost;Database=SingleSignOn;User=root;Password=Password1" in appsettings.json
- Run Microsoft Sql Server Migration & Seeding 

## Docker
Replace the 10.133.7.99 address with the actual ip / domain.

### Postgres

```
docker stop single-sign-on-admin-api 
docker rm single-sign-on-admin-api  
docker run \
	--name=single-sign-on-admin-api \
	-d --restart unless-stopped \
	-p 5001:80 \
	-e "ConnectionStrings__DefaultConnection"="Host=10.133.7.99;Database=SingleSignOn;Username=postgres;Password=password1;" \
	-e "Url__Authority"="http://10.133.7.99:5000" \
	-e "Url__CorsUrl"="http://10.133.7.99:4200" \
	-e "DatabaseType"="Postgres" \
	laredoza/single-sign-on-admin-api:latest
```

### SqlExpress

```
docker stop single-sign-on-admin-api 
docker rm single-sign-on-admin-api  
docker run \
	--name=single-sign-on-admin-api \
	-d --restart unless-stopped \
	-p 5001:80 \
	-e "ConnectionStrings__DefaultConnection"="Data Source=10.133.7.99;Initial Catalog=SingleSignOn;User ID=sa;Password=yourStrong(!)Password;" \
	-e "DatabaseType"="MsSql" \
	-e "Url__Authority"="http://10.133.7.99:5000" \
	-e "Url__CorsUrl"="http://10.133.7.99:4200" \
	single-sign-on-admin-api:latest \
	--restart unless-stopped
```

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

### SqlExpress

```
docker stop sql-express 
docker rm sql-express 
docker run \
	--name sql-express \
	-e 'ACCEPT_EULA=Y' \
	-e 'SA_PASSWORD=yourStrong(!)Password' \
	-e 'MSSQL_PID=Express' \
	-p 1433:1433 \
	-d --restart unless-stopped \
	mcr.microsoft.com/mssql/server:2017-latest-ubuntu
```

### MySql

```
docker stop my-sql
docker rm my-sql
docker run \
        --name my-sql \
        -d --restart unless-stopped \
        -e MYSQL_ROOT_PASSWORD=Password1 \
        -p 3306:3306 \
        -v /home/docker/my-sql/datadir:/var/lib/mysql \
        -v /home/docker/my-sql/config:/etc/mysql/conf.d \
        mysql:latest

```
