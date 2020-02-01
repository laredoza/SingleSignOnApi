CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(95) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
);

CREATE TABLE `DeviceCodes` (
    `UserCode` varchar(200) NOT NULL,
    `DeviceCode` varchar(200) NOT NULL,
    `SubjectId` varchar(200) NULL,
    `ClientId` varchar(200) NOT NULL,
    `CreationTime` datetime(6) NOT NULL,
    `Expiration` datetime(6) NOT NULL,
    `Data` longtext NOT NULL,
    CONSTRAINT `PK_DeviceCodes` PRIMARY KEY (`UserCode`)
);

CREATE TABLE `PersistedGrants` (
    `Key` varchar(200) NOT NULL,
    `Type` varchar(50) NOT NULL,
    `SubjectId` varchar(200) NULL,
    `ClientId` varchar(200) NOT NULL,
    `CreationTime` datetime(6) NOT NULL,
    `Expiration` datetime(6) NULL,
    `Data` longtext NOT NULL,
    CONSTRAINT `PK_PersistedGrants` PRIMARY KEY (`Key`)
);

CREATE UNIQUE INDEX `IX_DeviceCodes_DeviceCode` ON `DeviceCodes` (`DeviceCode`);

CREATE INDEX `IX_PersistedGrants_SubjectId_ClientId_Type` ON `PersistedGrants` (`SubjectId`, `ClientId`, `Type`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20200201134532_Initial', '2.2.6-servicing-10079');

