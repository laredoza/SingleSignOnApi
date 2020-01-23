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
- SqlServer ( Run the SingleSignOn.Migrations.SqlServer project. Not tested yet )