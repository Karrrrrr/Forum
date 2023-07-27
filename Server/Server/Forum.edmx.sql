
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/13/2021 15:54:09
-- Generated from EDMX file: D:\Документы\Шарага\ПТПМ\Лабораторные работы\Лабораторная работа 1\Лабораторная работа 8\Server\Server\Forum.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [D:\ДОКУМЕНТЫ\ШАРАГА\ПТПМ\ЛАБОРАТОРНЫЕ РАБОТЫ\ЛАБОРАТОРНАЯ РАБОТА 1\ЛАБОРАТОРНАЯ РАБОТА 8\SERVER\SERVER\BIN\DEBUG\FORUM.MDF];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UserAccount]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AccountSet] DROP CONSTRAINT [FK_UserAccount];
GO
IF OBJECT_ID(N'[dbo].[FK_AccountBranch]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BranchSet] DROP CONSTRAINT [FK_AccountBranch];
GO
IF OBJECT_ID(N'[dbo].[FK_SectionBranch]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BranchSet] DROP CONSTRAINT [FK_SectionBranch];
GO
IF OBJECT_ID(N'[dbo].[FK_BranchMessage]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MessageSet] DROP CONSTRAINT [FK_BranchMessage];
GO
IF OBJECT_ID(N'[dbo].[FK_AccountMessage]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MessageSet] DROP CONSTRAINT [FK_AccountMessage];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[UserSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserSet];
GO
IF OBJECT_ID(N'[dbo].[AccountSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AccountSet];
GO
IF OBJECT_ID(N'[dbo].[BranchSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BranchSet];
GO
IF OBJECT_ID(N'[dbo].[SectionSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SectionSet];
GO
IF OBJECT_ID(N'[dbo].[MessageSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MessageSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UserSet'
CREATE TABLE [dbo].[UserSet] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Login] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [Role] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'AccountSet'
CREATE TABLE [dbo].[AccountSet] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Birthday] datetime  NULL,
    [Picture] nvarchar(max)  NULL,
    [Gender] nvarchar(max)  NOT NULL,
    [User_ID] int  NOT NULL
);
GO

-- Creating table 'BranchSet'
CREATE TABLE [dbo].[BranchSet] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [AccessLevel] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [OwnerID] int  NOT NULL,
    [SectionID] int  NOT NULL,
    [Account_ID] int  NOT NULL,
    [Section_ID] int  NOT NULL
);
GO

-- Creating table 'SectionSet'
CREATE TABLE [dbo].[SectionSet] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [AccessLevel] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'MessageSet'
CREATE TABLE [dbo].[MessageSet] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Text] nvarchar(max)  NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [OwnerID] int  NOT NULL,
    [BranchID] int  NOT NULL,
    [Branch_ID] int  NOT NULL,
    [Account_ID] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'UserSet'
ALTER TABLE [dbo].[UserSet]
ADD CONSTRAINT [PK_UserSet]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'AccountSet'
ALTER TABLE [dbo].[AccountSet]
ADD CONSTRAINT [PK_AccountSet]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'BranchSet'
ALTER TABLE [dbo].[BranchSet]
ADD CONSTRAINT [PK_BranchSet]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'SectionSet'
ALTER TABLE [dbo].[SectionSet]
ADD CONSTRAINT [PK_SectionSet]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'MessageSet'
ALTER TABLE [dbo].[MessageSet]
ADD CONSTRAINT [PK_MessageSet]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [User_ID] in table 'AccountSet'
ALTER TABLE [dbo].[AccountSet]
ADD CONSTRAINT [FK_UserAccount]
    FOREIGN KEY ([User_ID])
    REFERENCES [dbo].[UserSet]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserAccount'
CREATE INDEX [IX_FK_UserAccount]
ON [dbo].[AccountSet]
    ([User_ID]);
GO

-- Creating foreign key on [Account_ID] in table 'BranchSet'
ALTER TABLE [dbo].[BranchSet]
ADD CONSTRAINT [FK_AccountBranch]
    FOREIGN KEY ([Account_ID])
    REFERENCES [dbo].[AccountSet]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AccountBranch'
CREATE INDEX [IX_FK_AccountBranch]
ON [dbo].[BranchSet]
    ([Account_ID]);
GO

-- Creating foreign key on [Section_ID] in table 'BranchSet'
ALTER TABLE [dbo].[BranchSet]
ADD CONSTRAINT [FK_SectionBranch]
    FOREIGN KEY ([Section_ID])
    REFERENCES [dbo].[SectionSet]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SectionBranch'
CREATE INDEX [IX_FK_SectionBranch]
ON [dbo].[BranchSet]
    ([Section_ID]);
GO

-- Creating foreign key on [Branch_ID] in table 'MessageSet'
ALTER TABLE [dbo].[MessageSet]
ADD CONSTRAINT [FK_BranchMessage]
    FOREIGN KEY ([Branch_ID])
    REFERENCES [dbo].[BranchSet]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BranchMessage'
CREATE INDEX [IX_FK_BranchMessage]
ON [dbo].[MessageSet]
    ([Branch_ID]);
GO

-- Creating foreign key on [Account_ID] in table 'MessageSet'
ALTER TABLE [dbo].[MessageSet]
ADD CONSTRAINT [FK_AccountMessage]
    FOREIGN KEY ([Account_ID])
    REFERENCES [dbo].[AccountSet]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AccountMessage'
CREATE INDEX [IX_FK_AccountMessage]
ON [dbo].[MessageSet]
    ([Account_ID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------