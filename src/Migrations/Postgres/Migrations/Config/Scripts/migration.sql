CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

CREATE TABLE "ApiResources" (
    "Id" serial NOT NULL,
    "Enabled" boolean NOT NULL,
    "Name" character varying(200) NOT NULL,
    "DisplayName" character varying(200) NULL,
    "Description" character varying(1000) NULL,
    "Created" timestamp without time zone NOT NULL,
    "Updated" timestamp without time zone NULL,
    "LastAccessed" timestamp without time zone NULL,
    "NonEditable" boolean NOT NULL,
    CONSTRAINT "PK_ApiResources" PRIMARY KEY ("Id")
);

CREATE TABLE "Clients" (
    "Id" serial NOT NULL,
    "Enabled" boolean NOT NULL,
    "ClientId" character varying(200) NOT NULL,
    "ProtocolType" character varying(200) NOT NULL,
    "RequireClientSecret" boolean NOT NULL,
    "ClientName" character varying(200) NULL,
    "Description" character varying(1000) NULL,
    "ClientUri" character varying(2000) NULL,
    "LogoUri" character varying(2000) NULL,
    "RequireConsent" boolean NOT NULL,
    "AllowRememberConsent" boolean NOT NULL,
    "AlwaysIncludeUserClaimsInIdToken" boolean NOT NULL,
    "RequirePkce" boolean NOT NULL,
    "AllowPlainTextPkce" boolean NOT NULL,
    "AllowAccessTokensViaBrowser" boolean NOT NULL,
    "FrontChannelLogoutUri" character varying(2000) NULL,
    "FrontChannelLogoutSessionRequired" boolean NOT NULL,
    "BackChannelLogoutUri" character varying(2000) NULL,
    "BackChannelLogoutSessionRequired" boolean NOT NULL,
    "AllowOfflineAccess" boolean NOT NULL,
    "IdentityTokenLifetime" integer NOT NULL,
    "AccessTokenLifetime" integer NOT NULL,
    "AuthorizationCodeLifetime" integer NOT NULL,
    "ConsentLifetime" integer NULL,
    "AbsoluteRefreshTokenLifetime" integer NOT NULL,
    "SlidingRefreshTokenLifetime" integer NOT NULL,
    "RefreshTokenUsage" integer NOT NULL,
    "UpdateAccessTokenClaimsOnRefresh" boolean NOT NULL,
    "RefreshTokenExpiration" integer NOT NULL,
    "AccessTokenType" integer NOT NULL,
    "EnableLocalLogin" boolean NOT NULL,
    "IncludeJwtId" boolean NOT NULL,
    "AlwaysSendClientClaims" boolean NOT NULL,
    "ClientClaimsPrefix" character varying(200) NULL,
    "PairWiseSubjectSalt" character varying(200) NULL,
    "Created" timestamp without time zone NOT NULL,
    "Updated" timestamp without time zone NULL,
    "LastAccessed" timestamp without time zone NULL,
    "UserSsoLifetime" integer NULL,
    "UserCodeType" character varying(100) NULL,
    "DeviceCodeLifetime" integer NOT NULL,
    "NonEditable" boolean NOT NULL,
    CONSTRAINT "PK_Clients" PRIMARY KEY ("Id")
);

CREATE TABLE "IdentityResources" (
    "Id" serial NOT NULL,
    "Enabled" boolean NOT NULL,
    "Name" character varying(200) NOT NULL,
    "DisplayName" character varying(200) NULL,
    "Description" character varying(1000) NULL,
    "Required" boolean NOT NULL,
    "Emphasize" boolean NOT NULL,
    "ShowInDiscoveryDocument" boolean NOT NULL,
    "Created" timestamp without time zone NOT NULL,
    "Updated" timestamp without time zone NULL,
    "NonEditable" boolean NOT NULL,
    CONSTRAINT "PK_IdentityResources" PRIMARY KEY ("Id")
);

CREATE TABLE "ApiClaims" (
    "Id" serial NOT NULL,
    "Type" character varying(200) NOT NULL,
    "ApiResourceId" integer NOT NULL,
    CONSTRAINT "PK_ApiClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ApiClaims_ApiResources_ApiResourceId" FOREIGN KEY ("ApiResourceId") REFERENCES "ApiResources" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ApiProperties" (
    "Id" serial NOT NULL,
    "Key" character varying(250) NOT NULL,
    "Value" character varying(2000) NOT NULL,
    "ApiResourceId" integer NOT NULL,
    CONSTRAINT "PK_ApiProperties" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ApiProperties_ApiResources_ApiResourceId" FOREIGN KEY ("ApiResourceId") REFERENCES "ApiResources" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ApiScopes" (
    "Id" serial NOT NULL,
    "Name" character varying(200) NOT NULL,
    "DisplayName" character varying(200) NULL,
    "Description" character varying(1000) NULL,
    "Required" boolean NOT NULL,
    "Emphasize" boolean NOT NULL,
    "ShowInDiscoveryDocument" boolean NOT NULL,
    "ApiResourceId" integer NOT NULL,
    CONSTRAINT "PK_ApiScopes" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ApiScopes_ApiResources_ApiResourceId" FOREIGN KEY ("ApiResourceId") REFERENCES "ApiResources" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ApiSecrets" (
    "Id" serial NOT NULL,
    "Description" character varying(1000) NULL,
    "Value" character varying(4000) NOT NULL,
    "Expiration" timestamp without time zone NULL,
    "Type" character varying(250) NOT NULL,
    "Created" timestamp without time zone NOT NULL,
    "ApiResourceId" integer NOT NULL,
    CONSTRAINT "PK_ApiSecrets" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ApiSecrets_ApiResources_ApiResourceId" FOREIGN KEY ("ApiResourceId") REFERENCES "ApiResources" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ClientClaims" (
    "Id" serial NOT NULL,
    "Type" character varying(250) NOT NULL,
    "Value" character varying(250) NOT NULL,
    "ClientId" integer NOT NULL,
    CONSTRAINT "PK_ClientClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ClientClaims_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "Clients" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ClientCorsOrigins" (
    "Id" serial NOT NULL,
    "Origin" character varying(150) NOT NULL,
    "ClientId" integer NOT NULL,
    CONSTRAINT "PK_ClientCorsOrigins" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ClientCorsOrigins_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "Clients" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ClientGrantTypes" (
    "Id" serial NOT NULL,
    "GrantType" character varying(250) NOT NULL,
    "ClientId" integer NOT NULL,
    CONSTRAINT "PK_ClientGrantTypes" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ClientGrantTypes_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "Clients" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ClientIdPRestrictions" (
    "Id" serial NOT NULL,
    "Provider" character varying(200) NOT NULL,
    "ClientId" integer NOT NULL,
    CONSTRAINT "PK_ClientIdPRestrictions" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ClientIdPRestrictions_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "Clients" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ClientPostLogoutRedirectUris" (
    "Id" serial NOT NULL,
    "PostLogoutRedirectUri" character varying(2000) NOT NULL,
    "ClientId" integer NOT NULL,
    CONSTRAINT "PK_ClientPostLogoutRedirectUris" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ClientPostLogoutRedirectUris_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "Clients" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ClientProperties" (
    "Id" serial NOT NULL,
    "Key" character varying(250) NOT NULL,
    "Value" character varying(2000) NOT NULL,
    "ClientId" integer NOT NULL,
    CONSTRAINT "PK_ClientProperties" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ClientProperties_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "Clients" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ClientRedirectUris" (
    "Id" serial NOT NULL,
    "RedirectUri" character varying(2000) NOT NULL,
    "ClientId" integer NOT NULL,
    CONSTRAINT "PK_ClientRedirectUris" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ClientRedirectUris_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "Clients" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ClientScopes" (
    "Id" serial NOT NULL,
    "Scope" character varying(200) NOT NULL,
    "ClientId" integer NOT NULL,
    CONSTRAINT "PK_ClientScopes" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ClientScopes_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "Clients" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ClientSecrets" (
    "Id" serial NOT NULL,
    "Description" character varying(2000) NULL,
    "Value" character varying(4000) NOT NULL,
    "Expiration" timestamp without time zone NULL,
    "Type" character varying(250) NOT NULL,
    "Created" timestamp without time zone NOT NULL,
    "ClientId" integer NOT NULL,
    CONSTRAINT "PK_ClientSecrets" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ClientSecrets_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "Clients" ("Id") ON DELETE CASCADE
);

CREATE TABLE "IdentityClaims" (
    "Id" serial NOT NULL,
    "Type" character varying(200) NOT NULL,
    "IdentityResourceId" integer NOT NULL,
    CONSTRAINT "PK_IdentityClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_IdentityClaims_IdentityResources_IdentityResourceId" FOREIGN KEY ("IdentityResourceId") REFERENCES "IdentityResources" ("Id") ON DELETE CASCADE
);

CREATE TABLE "IdentityProperties" (
    "Id" serial NOT NULL,
    "Key" character varying(250) NOT NULL,
    "Value" character varying(2000) NOT NULL,
    "IdentityResourceId" integer NOT NULL,
    CONSTRAINT "PK_IdentityProperties" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_IdentityProperties_IdentityResources_IdentityResourceId" FOREIGN KEY ("IdentityResourceId") REFERENCES "IdentityResources" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ApiScopeClaims" (
    "Id" serial NOT NULL,
    "Type" character varying(200) NOT NULL,
    "ApiScopeId" integer NOT NULL,
    CONSTRAINT "PK_ApiScopeClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ApiScopeClaims_ApiScopes_ApiScopeId" FOREIGN KEY ("ApiScopeId") REFERENCES "ApiScopes" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_ApiClaims_ApiResourceId" ON "ApiClaims" ("ApiResourceId");

CREATE INDEX "IX_ApiProperties_ApiResourceId" ON "ApiProperties" ("ApiResourceId");

CREATE UNIQUE INDEX "IX_ApiResources_Name" ON "ApiResources" ("Name");

CREATE INDEX "IX_ApiScopeClaims_ApiScopeId" ON "ApiScopeClaims" ("ApiScopeId");

CREATE INDEX "IX_ApiScopes_ApiResourceId" ON "ApiScopes" ("ApiResourceId");

CREATE UNIQUE INDEX "IX_ApiScopes_Name" ON "ApiScopes" ("Name");

CREATE INDEX "IX_ApiSecrets_ApiResourceId" ON "ApiSecrets" ("ApiResourceId");

CREATE INDEX "IX_ClientClaims_ClientId" ON "ClientClaims" ("ClientId");

CREATE INDEX "IX_ClientCorsOrigins_ClientId" ON "ClientCorsOrigins" ("ClientId");

CREATE INDEX "IX_ClientGrantTypes_ClientId" ON "ClientGrantTypes" ("ClientId");

CREATE INDEX "IX_ClientIdPRestrictions_ClientId" ON "ClientIdPRestrictions" ("ClientId");

CREATE INDEX "IX_ClientPostLogoutRedirectUris_ClientId" ON "ClientPostLogoutRedirectUris" ("ClientId");

CREATE INDEX "IX_ClientProperties_ClientId" ON "ClientProperties" ("ClientId");

CREATE INDEX "IX_ClientRedirectUris_ClientId" ON "ClientRedirectUris" ("ClientId");

CREATE UNIQUE INDEX "IX_Clients_ClientId" ON "Clients" ("ClientId");

CREATE INDEX "IX_ClientScopes_ClientId" ON "ClientScopes" ("ClientId");

CREATE INDEX "IX_ClientSecrets_ClientId" ON "ClientSecrets" ("ClientId");

CREATE INDEX "IX_IdentityClaims_IdentityResourceId" ON "IdentityClaims" ("IdentityResourceId");

CREATE INDEX "IX_IdentityProperties_IdentityResourceId" ON "IdentityProperties" ("IdentityResourceId");

CREATE UNIQUE INDEX "IX_IdentityResources_Name" ON "IdentityResources" ("Name");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20200101181147_Initial', '2.2.4-servicing-10062');

