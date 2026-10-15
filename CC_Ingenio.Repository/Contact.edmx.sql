
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 02/26/2014 14:03:25
-- Generated from EDMX file: D:\Sample Works\PersonalContact\Contact.Repository\Contact.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [TestMVC.Models.TestMVCDbContext];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Fk_ContactAddress_AddressId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ContactAddress] DROP CONSTRAINT [Fk_ContactAddress_AddressId];
GO
IF OBJECT_ID(N'[dbo].[Fk_ContactAddress_ContactId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ContactAddress] DROP CONSTRAINT [Fk_ContactAddress_ContactId];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Address]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Address];
GO
IF OBJECT_ID(N'[dbo].[Contact]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Contact];
GO
IF OBJECT_ID(N'[dbo].[ContactAddress]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ContactAddress];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Addresses'
CREATE TABLE [dbo].[Addresses] (
    [AddressId] int IDENTITY(1,1) NOT NULL,
    [Line1] varchar(100)  NULL,
    [Line2] varchar(100)  NULL,
    [City] varchar(25)  NULL,
    [State] varchar(25)  NULL,
    [Country] varchar(25)  NULL
);
GO

-- Creating table 'Contact'
CREATE TABLE [dbo].[Contact] (
    [ContactId] int IDENTITY(1,1) NOT NULL,
    [MobileNo] varchar(25)  NULL,
    [HomePhoneNo] varchar(25)  NULL,
    [OfficePhoneNo] varchar(25)  NULL,
    [Type] varchar(5)  NULL,
    [OrganizationName] varchar(5)  NULL,
    [FirstName] varchar(25)  NULL,
    [MiddleName] varchar(25)  NULL,
    [LastName] varchar(25)  NULL,
    [SSN] varchar(25)  NULL
);
GO

-- Creating table 'ContactAddresses'
CREATE TABLE [dbo].[ContactAddresses] (
    [ContactAddressId] int IDENTITY(1,1) NOT NULL,
    [ContactId] int  NULL,
    [AddressId] int  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [AddressId] in table 'Addresses'
ALTER TABLE [dbo].[Addresses]
ADD CONSTRAINT [PK_Addresses]
    PRIMARY KEY CLUSTERED ([AddressId] ASC);
GO

-- Creating primary key on [ContactId] in table 'Contact'
ALTER TABLE [dbo].[Contact]
ADD CONSTRAINT [PK_Contact]
    PRIMARY KEY CLUSTERED ([ContactId] ASC);
GO

-- Creating primary key on [ContactAddressId] in table 'ContactAddresses'
ALTER TABLE [dbo].[ContactAddresses]
ADD CONSTRAINT [PK_ContactAddresses]
    PRIMARY KEY CLUSTERED ([ContactAddressId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [AddressId] in table 'ContactAddresses'
ALTER TABLE [dbo].[ContactAddresses]
ADD CONSTRAINT [Fk_ContactAddress_AddressId]
    FOREIGN KEY ([AddressId])
    REFERENCES [dbo].[Addresses]
        ([AddressId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'Fk_ContactAddress_AddressId'
CREATE INDEX [IX_Fk_ContactAddress_AddressId]
ON [dbo].[ContactAddresses]
    ([AddressId]);
GO

-- Creating foreign key on [ContactId] in table 'ContactAddresses'
ALTER TABLE [dbo].[ContactAddresses]
ADD CONSTRAINT [Fk_ContactAddress_ContactId]
    FOREIGN KEY ([ContactId])
    REFERENCES [dbo].[Contact]
        ([ContactId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'Fk_ContactAddress_ContactId'
CREATE INDEX [IX_Fk_ContactAddress_ContactId]
ON [dbo].[ContactAddresses]
    ([ContactId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------