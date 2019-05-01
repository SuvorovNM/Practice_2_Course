
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/29/2019 20:02:05
-- Generated from EDMX file: K:\Practice\Practice\Models\Library.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [PracticeLib];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_AddressPerson]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AddressSet] DROP CONSTRAINT [FK_AddressPerson];
GO
IF OBJECT_ID(N'[dbo].[FK_PublisherPublication]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PublicationSet] DROP CONSTRAINT [FK_PublisherPublication];
GO
IF OBJECT_ID(N'[dbo].[FK_PenaltyBookReturning]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PenaltySet] DROP CONSTRAINT [FK_PenaltyBookReturning];
GO
IF OBJECT_ID(N'[dbo].[FK_BookReturningBookGiving]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BookReturningSet] DROP CONSTRAINT [FK_BookReturningBookGiving];
GO
IF OBJECT_ID(N'[dbo].[FK_ReaderBookGiving]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BookGivingSet] DROP CONSTRAINT [FK_ReaderBookGiving];
GO
IF OBJECT_ID(N'[dbo].[FK_LibrarianBookGiving]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BookGivingSet] DROP CONSTRAINT [FK_LibrarianBookGiving];
GO
IF OBJECT_ID(N'[dbo].[FK_PublicationBookGiving]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BookGivingSet] DROP CONSTRAINT [FK_PublicationBookGiving];
GO
IF OBJECT_ID(N'[dbo].[FK_LibrarianBookReturning]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BookReturningSet] DROP CONSTRAINT [FK_LibrarianBookReturning];
GO
IF OBJECT_ID(N'[dbo].[FK_Reader_inherits_Person]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PersonSet_Reader] DROP CONSTRAINT [FK_Reader_inherits_Person];
GO
IF OBJECT_ID(N'[dbo].[FK_Librarian_inherits_Person]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PersonSet_Librarian] DROP CONSTRAINT [FK_Librarian_inherits_Person];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[AddressSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AddressSet];
GO
IF OBJECT_ID(N'[dbo].[PersonSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PersonSet];
GO
IF OBJECT_ID(N'[dbo].[PublicationSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PublicationSet];
GO
IF OBJECT_ID(N'[dbo].[PublisherSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PublisherSet];
GO
IF OBJECT_ID(N'[dbo].[PenaltySet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PenaltySet];
GO
IF OBJECT_ID(N'[dbo].[BookGivingSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BookGivingSet];
GO
IF OBJECT_ID(N'[dbo].[BookReturningSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BookReturningSet];
GO
IF OBJECT_ID(N'[dbo].[PersonSet_Reader]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PersonSet_Reader];
GO
IF OBJECT_ID(N'[dbo].[PersonSet_Librarian]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PersonSet_Librarian];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'AddressSet'
CREATE TABLE [dbo].[AddressSet] (
    [Id] int  NOT NULL,
    [Region] nvarchar(max)  NOT NULL,
    [City] nvarchar(max)  NOT NULL,
    [Street] nvarchar(max)  NOT NULL,
    [House] nvarchar(max)  NOT NULL,
    [Flat] int  NOT NULL
);
GO

-- Creating table 'PersonSet'
CREATE TABLE [dbo].[PersonSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FIO] nvarchar(max)  NOT NULL,
    [Birthday] datetime  NOT NULL,
    [Phone_Number] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NULL
);
GO

-- Creating table 'PublicationSet'
CREATE TABLE [dbo].[PublicationSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [BBK] nvarchar(max)  NOT NULL,
    [UDK] nvarchar(max)  NOT NULL,
    [Author] nvarchar(max)  NOT NULL,
    [Available] bit  NOT NULL,
    [ISBN] nvarchar(max)  NOT NULL,
    [Page_Count] int  NOT NULL,
    [Release_Number] nvarchar(max)  NULL,
    [Year] int  NOT NULL,
    [Publisher_Id] int  NOT NULL
);
GO

-- Creating table 'PublisherSet'
CREATE TABLE [dbo].[PublisherSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [City] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'PenaltySet'
CREATE TABLE [dbo].[PenaltySet] (
    [Id] int  NOT NULL,
    [Info] nvarchar(max)  NOT NULL,
    [Sum] decimal(18,0)  NOT NULL
);
GO

-- Creating table 'BookGivingSet'
CREATE TABLE [dbo].[BookGivingSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Give_Date] datetime  NOT NULL,
    [Expected_Return_Date] datetime  NOT NULL,
    [Reader_Id] int  NOT NULL,
    [Librarian_Id] int  NOT NULL,
    [Publication_Id] int  NOT NULL
);
GO

-- Creating table 'BookReturningSet'
CREATE TABLE [dbo].[BookReturningSet] (
    [Id] int  NOT NULL,
    [Real_Return_Date] datetime  NOT NULL,
    [Librarian_Id] int  NOT NULL
);
GO

-- Creating table 'PersonSet_Reader'
CREATE TABLE [dbo].[PersonSet_Reader] (
    [Library_Card] int  NOT NULL,
    [Registration_Date] datetime  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'PersonSet_Librarian'
CREATE TABLE [dbo].[PersonSet_Librarian] (
    [Staff_Number] int  NOT NULL,
    [Hiring_Date] datetime  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [Privilege] tinyint  NOT NULL,
    [Deleted] bit  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'AddressSet'
ALTER TABLE [dbo].[AddressSet]
ADD CONSTRAINT [PK_AddressSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PersonSet'
ALTER TABLE [dbo].[PersonSet]
ADD CONSTRAINT [PK_PersonSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PublicationSet'
ALTER TABLE [dbo].[PublicationSet]
ADD CONSTRAINT [PK_PublicationSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PublisherSet'
ALTER TABLE [dbo].[PublisherSet]
ADD CONSTRAINT [PK_PublisherSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PenaltySet'
ALTER TABLE [dbo].[PenaltySet]
ADD CONSTRAINT [PK_PenaltySet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BookGivingSet'
ALTER TABLE [dbo].[BookGivingSet]
ADD CONSTRAINT [PK_BookGivingSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BookReturningSet'
ALTER TABLE [dbo].[BookReturningSet]
ADD CONSTRAINT [PK_BookReturningSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PersonSet_Reader'
ALTER TABLE [dbo].[PersonSet_Reader]
ADD CONSTRAINT [PK_PersonSet_Reader]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PersonSet_Librarian'
ALTER TABLE [dbo].[PersonSet_Librarian]
ADD CONSTRAINT [PK_PersonSet_Librarian]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Id] in table 'AddressSet'
ALTER TABLE [dbo].[AddressSet]
ADD CONSTRAINT [FK_AddressPerson]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[PersonSet]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Publisher_Id] in table 'PublicationSet'
ALTER TABLE [dbo].[PublicationSet]
ADD CONSTRAINT [FK_PublisherPublication]
    FOREIGN KEY ([Publisher_Id])
    REFERENCES [dbo].[PublisherSet]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PublisherPublication'
CREATE INDEX [IX_FK_PublisherPublication]
ON [dbo].[PublicationSet]
    ([Publisher_Id]);
GO

-- Creating foreign key on [Id] in table 'PenaltySet'
ALTER TABLE [dbo].[PenaltySet]
ADD CONSTRAINT [FK_PenaltyBookReturning]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[BookReturningSet]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'BookReturningSet'
ALTER TABLE [dbo].[BookReturningSet]
ADD CONSTRAINT [FK_BookReturningBookGiving]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[BookGivingSet]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Reader_Id] in table 'BookGivingSet'
ALTER TABLE [dbo].[BookGivingSet]
ADD CONSTRAINT [FK_ReaderBookGiving]
    FOREIGN KEY ([Reader_Id])
    REFERENCES [dbo].[PersonSet_Reader]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReaderBookGiving'
CREATE INDEX [IX_FK_ReaderBookGiving]
ON [dbo].[BookGivingSet]
    ([Reader_Id]);
GO

-- Creating foreign key on [Librarian_Id] in table 'BookGivingSet'
ALTER TABLE [dbo].[BookGivingSet]
ADD CONSTRAINT [FK_LibrarianBookGiving]
    FOREIGN KEY ([Librarian_Id])
    REFERENCES [dbo].[PersonSet_Librarian]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_LibrarianBookGiving'
CREATE INDEX [IX_FK_LibrarianBookGiving]
ON [dbo].[BookGivingSet]
    ([Librarian_Id]);
GO

-- Creating foreign key on [Publication_Id] in table 'BookGivingSet'
ALTER TABLE [dbo].[BookGivingSet]
ADD CONSTRAINT [FK_PublicationBookGiving]
    FOREIGN KEY ([Publication_Id])
    REFERENCES [dbo].[PublicationSet]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PublicationBookGiving'
CREATE INDEX [IX_FK_PublicationBookGiving]
ON [dbo].[BookGivingSet]
    ([Publication_Id]);
GO

-- Creating foreign key on [Librarian_Id] in table 'BookReturningSet'
ALTER TABLE [dbo].[BookReturningSet]
ADD CONSTRAINT [FK_LibrarianBookReturning]
    FOREIGN KEY ([Librarian_Id])
    REFERENCES [dbo].[PersonSet_Librarian]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_LibrarianBookReturning'
CREATE INDEX [IX_FK_LibrarianBookReturning]
ON [dbo].[BookReturningSet]
    ([Librarian_Id]);
GO

-- Creating foreign key on [Id] in table 'PersonSet_Reader'
ALTER TABLE [dbo].[PersonSet_Reader]
ADD CONSTRAINT [FK_Reader_inherits_Person]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[PersonSet]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'PersonSet_Librarian'
ALTER TABLE [dbo].[PersonSet_Librarian]
ADD CONSTRAINT [FK_Librarian_inherits_Person]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[PersonSet]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------