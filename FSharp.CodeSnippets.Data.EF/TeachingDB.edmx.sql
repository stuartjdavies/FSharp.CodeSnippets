
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/03/2014 21:27:39
-- Generated from EDMX file: C:\Users\stuart\Documents\GitHub\FSharp.CodeSnippets\FSharp.CodeSnippets.Data.EF\TeachingDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [TeachingDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_GroupsStudents]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Students] DROP CONSTRAINT [FK_GroupsStudents];
GO
IF OBJECT_ID(N'[dbo].[FK_GroupsSubj_Teach]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Subj_Teach] DROP CONSTRAINT [FK_GroupsSubj_Teach];
GO
IF OBJECT_ID(N'[dbo].[FK_StudentMark]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Marks] DROP CONSTRAINT [FK_StudentMark];
GO
IF OBJECT_ID(N'[dbo].[FK_SubjectMark]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Marks] DROP CONSTRAINT [FK_SubjectMark];
GO
IF OBJECT_ID(N'[dbo].[FK_SubjectsSubj_Teach]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Subj_Teach] DROP CONSTRAINT [FK_SubjectsSubj_Teach];
GO
IF OBJECT_ID(N'[dbo].[FK_TeachersSubj_Teach]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Subj_Teach] DROP CONSTRAINT [FK_TeachersSubj_Teach];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Groups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Groups];
GO
IF OBJECT_ID(N'[dbo].[Marks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Marks];
GO
IF OBJECT_ID(N'[dbo].[Students]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Students];
GO
IF OBJECT_ID(N'[dbo].[Subj_Teach]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Subj_Teach];
GO
IF OBJECT_ID(N'[dbo].[Subjects]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Subjects];
GO
IF OBJECT_ID(N'[dbo].[Teachers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Teachers];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Groups'
CREATE TABLE [dbo].[Groups] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Marks'
CREATE TABLE [dbo].[Marks] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Date] datetime  NOT NULL,
    [StudentMarks] int  NOT NULL,
    [Student_Id] int  NOT NULL,
    [Subject_Id] int  NOT NULL
);
GO

-- Creating table 'Students'
CREATE TABLE [dbo].[Students] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [Group_Id] int  NOT NULL
);
GO

-- Creating table 'Subj_Teach'
CREATE TABLE [dbo].[Subj_Teach] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Teacher_Id] int  NOT NULL,
    [Subject_Id] int  NOT NULL,
    [Group_Id] int  NOT NULL
);
GO

-- Creating table 'Subjects'
CREATE TABLE [dbo].[Subjects] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Teachers'
CREATE TABLE [dbo].[Teachers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Groups'
ALTER TABLE [dbo].[Groups]
ADD CONSTRAINT [PK_Groups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Marks'
ALTER TABLE [dbo].[Marks]
ADD CONSTRAINT [PK_Marks]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Students'
ALTER TABLE [dbo].[Students]
ADD CONSTRAINT [PK_Students]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Subj_Teach'
ALTER TABLE [dbo].[Subj_Teach]
ADD CONSTRAINT [PK_Subj_Teach]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Subjects'
ALTER TABLE [dbo].[Subjects]
ADD CONSTRAINT [PK_Subjects]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Teachers'
ALTER TABLE [dbo].[Teachers]
ADD CONSTRAINT [PK_Teachers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Group_Id] in table 'Students'
ALTER TABLE [dbo].[Students]
ADD CONSTRAINT [FK_GroupsStudents]
    FOREIGN KEY ([Group_Id])
    REFERENCES [dbo].[Groups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GroupsStudents'
CREATE INDEX [IX_FK_GroupsStudents]
ON [dbo].[Students]
    ([Group_Id]);
GO

-- Creating foreign key on [Group_Id] in table 'Subj_Teach'
ALTER TABLE [dbo].[Subj_Teach]
ADD CONSTRAINT [FK_GroupsSubj_Teach]
    FOREIGN KEY ([Group_Id])
    REFERENCES [dbo].[Groups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GroupsSubj_Teach'
CREATE INDEX [IX_FK_GroupsSubj_Teach]
ON [dbo].[Subj_Teach]
    ([Group_Id]);
GO

-- Creating foreign key on [Student_Id] in table 'Marks'
ALTER TABLE [dbo].[Marks]
ADD CONSTRAINT [FK_StudentMark]
    FOREIGN KEY ([Student_Id])
    REFERENCES [dbo].[Students]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StudentMark'
CREATE INDEX [IX_FK_StudentMark]
ON [dbo].[Marks]
    ([Student_Id]);
GO

-- Creating foreign key on [Subject_Id] in table 'Marks'
ALTER TABLE [dbo].[Marks]
ADD CONSTRAINT [FK_SubjectMark]
    FOREIGN KEY ([Subject_Id])
    REFERENCES [dbo].[Subjects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SubjectMark'
CREATE INDEX [IX_FK_SubjectMark]
ON [dbo].[Marks]
    ([Subject_Id]);
GO

-- Creating foreign key on [Subject_Id] in table 'Subj_Teach'
ALTER TABLE [dbo].[Subj_Teach]
ADD CONSTRAINT [FK_SubjectsSubj_Teach]
    FOREIGN KEY ([Subject_Id])
    REFERENCES [dbo].[Subjects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SubjectsSubj_Teach'
CREATE INDEX [IX_FK_SubjectsSubj_Teach]
ON [dbo].[Subj_Teach]
    ([Subject_Id]);
GO

-- Creating foreign key on [Teacher_Id] in table 'Subj_Teach'
ALTER TABLE [dbo].[Subj_Teach]
ADD CONSTRAINT [FK_TeachersSubj_Teach]
    FOREIGN KEY ([Teacher_Id])
    REFERENCES [dbo].[Teachers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TeachersSubj_Teach'
CREATE INDEX [IX_FK_TeachersSubj_Teach]
ON [dbo].[Subj_Teach]
    ([Teacher_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------