
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 02/17/2019 17:05:46
-- Generated from EDMX file: C:\Users\bogda\Desktop\Dekanat-master\DekanatForTeacher\DekanatForTeacher\ModelStudentMarks.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [dbStudentsFinal];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_AssociatedSubjects_Groups]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AssociatedSubjects] DROP CONSTRAINT [FK_AssociatedSubjects_Groups];
GO
IF OBJECT_ID(N'[dbo].[FK_AssociatedSubjects_SubjectInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AssociatedSubjects] DROP CONSTRAINT [FK_AssociatedSubjects_SubjectInfo];
GO
IF OBJECT_ID(N'[dbo].[FK_AssociatedSubjects_Teachers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AssociatedSubjects] DROP CONSTRAINT [FK_AssociatedSubjects_Teachers];
GO
IF OBJECT_ID(N'[dbo].[FK_Courses_Departments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Courses] DROP CONSTRAINT [FK_Courses_Departments];
GO
IF OBJECT_ID(N'[dbo].[FK_Credentials_Teachers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Credentials] DROP CONSTRAINT [FK_Credentials_Teachers];
GO
IF OBJECT_ID(N'[dbo].[FK_Departments_Institutes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Departments] DROP CONSTRAINT [FK_Departments_Institutes];
GO
IF OBJECT_ID(N'[dbo].[FK_Groups_Courses]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Groups] DROP CONSTRAINT [FK_Groups_Courses];
GO
IF OBJECT_ID(N'[dbo].[FK_Groups_QualificationLevels]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Groups] DROP CONSTRAINT [FK_Groups_QualificationLevels];
GO
IF OBJECT_ID(N'[dbo].[FK_Groups_Students]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Groups] DROP CONSTRAINT [FK_Groups_Students];
GO
IF OBJECT_ID(N'[dbo].[FK_Groups_StudyForms]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Groups] DROP CONSTRAINT [FK_Groups_StudyForms];
GO
IF OBJECT_ID(N'[dbo].[FK_Marks_MarkTypes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Marks] DROP CONSTRAINT [FK_Marks_MarkTypes];
GO
IF OBJECT_ID(N'[dbo].[FK_Marks_Students]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Marks] DROP CONSTRAINT [FK_Marks_Students];
GO
IF OBJECT_ID(N'[dbo].[FK_Marks_SubjectInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Marks] DROP CONSTRAINT [FK_Marks_SubjectInfo];
GO
IF OBJECT_ID(N'[dbo].[FK_Students_Groups]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Students] DROP CONSTRAINT [FK_Students_Groups];
GO
IF OBJECT_ID(N'[dbo].[FK_SubjectInfo_Courses]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SubjectInfo] DROP CONSTRAINT [FK_SubjectInfo_Courses];
GO
IF OBJECT_ID(N'[dbo].[FK_SubjectInfo_QualificationLevels]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SubjectInfo] DROP CONSTRAINT [FK_SubjectInfo_QualificationLevels];
GO
IF OBJECT_ID(N'[dbo].[FK_SubjectInfo_StudyForms]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SubjectInfo] DROP CONSTRAINT [FK_SubjectInfo_StudyForms];
GO
IF OBJECT_ID(N'[dbo].[FK_SubjectInfo_SubjectNames]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SubjectInfo] DROP CONSTRAINT [FK_SubjectInfo_SubjectNames];
GO
IF OBJECT_ID(N'[dbo].[FK_Teachers_AcademicRanks]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Teachers] DROP CONSTRAINT [FK_Teachers_AcademicRanks];
GO
IF OBJECT_ID(N'[dbo].[FK_Teachers_Departments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Teachers] DROP CONSTRAINT [FK_Teachers_Departments];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[AcademicRanks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AcademicRanks];
GO
IF OBJECT_ID(N'[dbo].[AssociatedSubjects]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AssociatedSubjects];
GO
IF OBJECT_ID(N'[dbo].[Courses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Courses];
GO
IF OBJECT_ID(N'[dbo].[Credentials]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Credentials];
GO
IF OBJECT_ID(N'[dbo].[DeletedMarks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DeletedMarks];
GO
IF OBJECT_ID(N'[dbo].[DeletedStudents]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DeletedStudents];
GO
IF OBJECT_ID(N'[dbo].[Departments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Departments];
GO
IF OBJECT_ID(N'[dbo].[Groups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Groups];
GO
IF OBJECT_ID(N'[dbo].[Institutes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Institutes];
GO
IF OBJECT_ID(N'[dbo].[Marks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Marks];
GO
IF OBJECT_ID(N'[dbo].[MarkTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MarkTypes];
GO
IF OBJECT_ID(N'[dbo].[QualificationLevels]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QualificationLevels];
GO
IF OBJECT_ID(N'[dbo].[Students]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Students];
GO
IF OBJECT_ID(N'[dbo].[StudyForms]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StudyForms];
GO
IF OBJECT_ID(N'[dbo].[SubjectInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SubjectInfo];
GO
IF OBJECT_ID(N'[dbo].[SubjectNames]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SubjectNames];
GO
IF OBJECT_ID(N'[dbo].[sysdiagrams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[sysdiagrams];
GO
IF OBJECT_ID(N'[dbo].[Teachers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Teachers];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'AcademicRanks'
CREATE TABLE [dbo].[AcademicRanks] (
    [AcademicRankId] int  NOT NULL,
    [AcademicRankName] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'AssociatedSubjects'
CREATE TABLE [dbo].[AssociatedSubjects] (
    [AssociatedSubjectId] int IDENTITY(1,1) NOT NULL,
    [Teacher] int  NOT NULL,
    [SubjectInfo] int  NOT NULL,
    [StudyGroup] int  NOT NULL
);
GO

-- Creating table 'Courses'
CREATE TABLE [dbo].[Courses] (
    [CourseId] int  NOT NULL,
    [CourseName] nvarchar(50)  NOT NULL,
    [Department] int  NOT NULL
);
GO

-- Creating table 'Credentials'
CREATE TABLE [dbo].[Credentials] (
    [CredentialId] int IDENTITY(1,1) NOT NULL,
    [Teacher] int  NOT NULL,
    [Username] varchar(25)  NOT NULL,
    [Password] varchar(40)  NOT NULL
);
GO

-- Creating table 'DeletedMarks'
CREATE TABLE [dbo].[DeletedMarks] (
    [MarkId] int  NOT NULL,
    [Student] int  NOT NULL,
    [SubjectInfo] int  NOT NULL,
    [MarkDate] datetime  NULL,
    [Mark] int  NULL,
    [MarkType] int  NOT NULL,
    [Status] int  NULL
);
GO

-- Creating table 'DeletedStudents'
CREATE TABLE [dbo].[DeletedStudents] (
    [StudentId] int  NOT NULL,
    [FirstName] nvarchar(50)  NOT NULL,
    [LastName] nvarchar(50)  NOT NULL,
    [MiddleName] nvarchar(50)  NOT NULL,
    [BirthDate] datetime  NOT NULL,
    [Address] nvarchar(50)  NOT NULL,
    [MobilePhone] varchar(13)  NULL,
    [StudyGroup] int  NOT NULL,
    [DeletedBy] varchar(25)  NOT NULL,
    [DateOfDeleting] datetime  NOT NULL
);
GO

-- Creating table 'Departments'
CREATE TABLE [dbo].[Departments] (
    [DepartmentId] int  NOT NULL,
    [DepartmentName] nvarchar(100)  NOT NULL,
    [Institute] int  NOT NULL
);
GO

-- Creating table 'Groups'
CREATE TABLE [dbo].[Groups] (
    [GroupId] int IDENTITY(1,1) NOT NULL,
    [GroupNum] int  NOT NULL,
    [GroupQuantity] int  NOT NULL,
    [Elder] int  NULL,
    [Course] int  NOT NULL,
    [QualificationLevel] int  NOT NULL,
    [StudyForm] int  NOT NULL
);
GO

-- Creating table 'Institutes'
CREATE TABLE [dbo].[Institutes] (
    [InstituteId] int  NOT NULL,
    [InstituteName] nvarchar(100)  NOT NULL,
    [InstituteSName] nvarchar(4)  NOT NULL
);
GO

-- Creating table 'Marks'
CREATE TABLE [dbo].[Marks] (
    [MarkId] int  NOT NULL,
    [Student] int  NOT NULL,
    [SubjectInfo] int  NOT NULL,
    [MarkDate] datetime  NULL,
    [Mark] int  NULL,
    [MarkType] int  NOT NULL
);
GO

-- Creating table 'MarkTypes'
CREATE TABLE [dbo].[MarkTypes] (
    [MarkTypeId] int  NOT NULL,
    [MarkTypeName] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'QualificationLevels'
CREATE TABLE [dbo].[QualificationLevels] (
    [QualificationLevelId] int  NOT NULL,
    [QualificationLevelName] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Students'
CREATE TABLE [dbo].[Students] (
    [StudentId] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(50)  NOT NULL,
    [LastName] nvarchar(50)  NOT NULL,
    [MiddleName] nvarchar(50)  NOT NULL,
    [BirthDate] datetime  NOT NULL,
    [Address] nvarchar(50)  NOT NULL,
    [MobilePhone] varchar(13)  NULL,
    [StudyGroup] int  NOT NULL
);
GO

-- Creating table 'StudyForms'
CREATE TABLE [dbo].[StudyForms] (
    [StudyFormId] int  NOT NULL,
    [StudyFormName] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'SubjectInfo'
CREATE TABLE [dbo].[SubjectInfo] (
    [SubjectInfoId] int IDENTITY(1,1) NOT NULL,
    [Credits] decimal(2,1)  NOT NULL,
    [QualificationLevel] int  NOT NULL,
    [StudyForm] int  NOT NULL,
    [Course] int  NOT NULL,
    [SubjectInfoDate] datetime  NOT NULL,
    [SubjectName] int  NOT NULL
);
GO

-- Creating table 'SubjectNames'
CREATE TABLE [dbo].[SubjectNames] (
    [SubjectNameId] int IDENTITY(1,1) NOT NULL,
    [SubjectName] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'sysdiagrams'
CREATE TABLE [dbo].[sysdiagrams] (
    [name] nvarchar(128)  NOT NULL,
    [principal_id] int  NOT NULL,
    [diagram_id] int IDENTITY(1,1) NOT NULL,
    [version] int  NULL,
    [definition] varbinary(max)  NULL
);
GO

-- Creating table 'Teachers'
CREATE TABLE [dbo].[Teachers] (
    [TeacherId] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(50)  NOT NULL,
    [LastName] nvarchar(50)  NOT NULL,
    [MiddleName] nvarchar(50)  NOT NULL,
    [BirthDate] datetime  NOT NULL,
    [Address] nvarchar(50)  NOT NULL,
    [MobilePhone] nvarchar(13)  NULL,
    [Department] int  NOT NULL,
    [AcademicRank] int  NOT NULL,
    [HasCredential] bit  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [AcademicRankId] in table 'AcademicRanks'
ALTER TABLE [dbo].[AcademicRanks]
ADD CONSTRAINT [PK_AcademicRanks]
    PRIMARY KEY CLUSTERED ([AcademicRankId] ASC);
GO

-- Creating primary key on [AssociatedSubjectId] in table 'AssociatedSubjects'
ALTER TABLE [dbo].[AssociatedSubjects]
ADD CONSTRAINT [PK_AssociatedSubjects]
    PRIMARY KEY CLUSTERED ([AssociatedSubjectId] ASC);
GO

-- Creating primary key on [CourseId] in table 'Courses'
ALTER TABLE [dbo].[Courses]
ADD CONSTRAINT [PK_Courses]
    PRIMARY KEY CLUSTERED ([CourseId] ASC);
GO

-- Creating primary key on [CredentialId] in table 'Credentials'
ALTER TABLE [dbo].[Credentials]
ADD CONSTRAINT [PK_Credentials]
    PRIMARY KEY CLUSTERED ([CredentialId] ASC);
GO

-- Creating primary key on [MarkId] in table 'DeletedMarks'
ALTER TABLE [dbo].[DeletedMarks]
ADD CONSTRAINT [PK_DeletedMarks]
    PRIMARY KEY CLUSTERED ([MarkId] ASC);
GO

-- Creating primary key on [StudentId] in table 'DeletedStudents'
ALTER TABLE [dbo].[DeletedStudents]
ADD CONSTRAINT [PK_DeletedStudents]
    PRIMARY KEY CLUSTERED ([StudentId] ASC);
GO

-- Creating primary key on [DepartmentId] in table 'Departments'
ALTER TABLE [dbo].[Departments]
ADD CONSTRAINT [PK_Departments]
    PRIMARY KEY CLUSTERED ([DepartmentId] ASC);
GO

-- Creating primary key on [GroupId] in table 'Groups'
ALTER TABLE [dbo].[Groups]
ADD CONSTRAINT [PK_Groups]
    PRIMARY KEY CLUSTERED ([GroupId] ASC);
GO

-- Creating primary key on [InstituteId] in table 'Institutes'
ALTER TABLE [dbo].[Institutes]
ADD CONSTRAINT [PK_Institutes]
    PRIMARY KEY CLUSTERED ([InstituteId] ASC);
GO

-- Creating primary key on [MarkId] in table 'Marks'
ALTER TABLE [dbo].[Marks]
ADD CONSTRAINT [PK_Marks]
    PRIMARY KEY CLUSTERED ([MarkId] ASC);
GO

-- Creating primary key on [MarkTypeId] in table 'MarkTypes'
ALTER TABLE [dbo].[MarkTypes]
ADD CONSTRAINT [PK_MarkTypes]
    PRIMARY KEY CLUSTERED ([MarkTypeId] ASC);
GO

-- Creating primary key on [QualificationLevelId] in table 'QualificationLevels'
ALTER TABLE [dbo].[QualificationLevels]
ADD CONSTRAINT [PK_QualificationLevels]
    PRIMARY KEY CLUSTERED ([QualificationLevelId] ASC);
GO

-- Creating primary key on [StudentId] in table 'Students'
ALTER TABLE [dbo].[Students]
ADD CONSTRAINT [PK_Students]
    PRIMARY KEY CLUSTERED ([StudentId] ASC);
GO

-- Creating primary key on [StudyFormId] in table 'StudyForms'
ALTER TABLE [dbo].[StudyForms]
ADD CONSTRAINT [PK_StudyForms]
    PRIMARY KEY CLUSTERED ([StudyFormId] ASC);
GO

-- Creating primary key on [SubjectInfoId] in table 'SubjectInfo'
ALTER TABLE [dbo].[SubjectInfo]
ADD CONSTRAINT [PK_SubjectInfo]
    PRIMARY KEY CLUSTERED ([SubjectInfoId] ASC);
GO

-- Creating primary key on [SubjectNameId] in table 'SubjectNames'
ALTER TABLE [dbo].[SubjectNames]
ADD CONSTRAINT [PK_SubjectNames]
    PRIMARY KEY CLUSTERED ([SubjectNameId] ASC);
GO

-- Creating primary key on [diagram_id] in table 'sysdiagrams'
ALTER TABLE [dbo].[sysdiagrams]
ADD CONSTRAINT [PK_sysdiagrams]
    PRIMARY KEY CLUSTERED ([diagram_id] ASC);
GO

-- Creating primary key on [TeacherId] in table 'Teachers'
ALTER TABLE [dbo].[Teachers]
ADD CONSTRAINT [PK_Teachers]
    PRIMARY KEY CLUSTERED ([TeacherId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [AcademicRank] in table 'Teachers'
ALTER TABLE [dbo].[Teachers]
ADD CONSTRAINT [FK_Teachers_AcademicRanks]
    FOREIGN KEY ([AcademicRank])
    REFERENCES [dbo].[AcademicRanks]
        ([AcademicRankId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Teachers_AcademicRanks'
CREATE INDEX [IX_FK_Teachers_AcademicRanks]
ON [dbo].[Teachers]
    ([AcademicRank]);
GO

-- Creating foreign key on [StudyGroup] in table 'AssociatedSubjects'
ALTER TABLE [dbo].[AssociatedSubjects]
ADD CONSTRAINT [FK_AssociatedSubjects_Groups]
    FOREIGN KEY ([StudyGroup])
    REFERENCES [dbo].[Groups]
        ([GroupId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AssociatedSubjects_Groups'
CREATE INDEX [IX_FK_AssociatedSubjects_Groups]
ON [dbo].[AssociatedSubjects]
    ([StudyGroup]);
GO

-- Creating foreign key on [SubjectInfo] in table 'AssociatedSubjects'
ALTER TABLE [dbo].[AssociatedSubjects]
ADD CONSTRAINT [FK_AssociatedSubjects_SubjectInfo]
    FOREIGN KEY ([SubjectInfo])
    REFERENCES [dbo].[SubjectInfo]
        ([SubjectInfoId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AssociatedSubjects_SubjectInfo'
CREATE INDEX [IX_FK_AssociatedSubjects_SubjectInfo]
ON [dbo].[AssociatedSubjects]
    ([SubjectInfo]);
GO

-- Creating foreign key on [Teacher] in table 'AssociatedSubjects'
ALTER TABLE [dbo].[AssociatedSubjects]
ADD CONSTRAINT [FK_AssociatedSubjects_Teachers]
    FOREIGN KEY ([Teacher])
    REFERENCES [dbo].[Teachers]
        ([TeacherId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AssociatedSubjects_Teachers'
CREATE INDEX [IX_FK_AssociatedSubjects_Teachers]
ON [dbo].[AssociatedSubjects]
    ([Teacher]);
GO

-- Creating foreign key on [Department] in table 'Courses'
ALTER TABLE [dbo].[Courses]
ADD CONSTRAINT [FK_Courses_Departments]
    FOREIGN KEY ([Department])
    REFERENCES [dbo].[Departments]
        ([DepartmentId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Courses_Departments'
CREATE INDEX [IX_FK_Courses_Departments]
ON [dbo].[Courses]
    ([Department]);
GO

-- Creating foreign key on [Course] in table 'Groups'
ALTER TABLE [dbo].[Groups]
ADD CONSTRAINT [FK_Groups_Courses]
    FOREIGN KEY ([Course])
    REFERENCES [dbo].[Courses]
        ([CourseId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Groups_Courses'
CREATE INDEX [IX_FK_Groups_Courses]
ON [dbo].[Groups]
    ([Course]);
GO

-- Creating foreign key on [Course] in table 'SubjectInfo'
ALTER TABLE [dbo].[SubjectInfo]
ADD CONSTRAINT [FK_SubjectInfo_Courses]
    FOREIGN KEY ([Course])
    REFERENCES [dbo].[Courses]
        ([CourseId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SubjectInfo_Courses'
CREATE INDEX [IX_FK_SubjectInfo_Courses]
ON [dbo].[SubjectInfo]
    ([Course]);
GO

-- Creating foreign key on [Teacher] in table 'Credentials'
ALTER TABLE [dbo].[Credentials]
ADD CONSTRAINT [FK_Credentials_Teachers]
    FOREIGN KEY ([Teacher])
    REFERENCES [dbo].[Teachers]
        ([TeacherId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Credentials_Teachers'
CREATE INDEX [IX_FK_Credentials_Teachers]
ON [dbo].[Credentials]
    ([Teacher]);
GO

-- Creating foreign key on [Institute] in table 'Departments'
ALTER TABLE [dbo].[Departments]
ADD CONSTRAINT [FK_Departments_Institutes]
    FOREIGN KEY ([Institute])
    REFERENCES [dbo].[Institutes]
        ([InstituteId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Departments_Institutes'
CREATE INDEX [IX_FK_Departments_Institutes]
ON [dbo].[Departments]
    ([Institute]);
GO

-- Creating foreign key on [Department] in table 'Teachers'
ALTER TABLE [dbo].[Teachers]
ADD CONSTRAINT [FK_Teachers_Departments]
    FOREIGN KEY ([Department])
    REFERENCES [dbo].[Departments]
        ([DepartmentId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Teachers_Departments'
CREATE INDEX [IX_FK_Teachers_Departments]
ON [dbo].[Teachers]
    ([Department]);
GO

-- Creating foreign key on [QualificationLevel] in table 'Groups'
ALTER TABLE [dbo].[Groups]
ADD CONSTRAINT [FK_Groups_QualificationLevels]
    FOREIGN KEY ([QualificationLevel])
    REFERENCES [dbo].[QualificationLevels]
        ([QualificationLevelId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Groups_QualificationLevels'
CREATE INDEX [IX_FK_Groups_QualificationLevels]
ON [dbo].[Groups]
    ([QualificationLevel]);
GO

-- Creating foreign key on [Elder] in table 'Groups'
ALTER TABLE [dbo].[Groups]
ADD CONSTRAINT [FK_Groups_Students]
    FOREIGN KEY ([Elder])
    REFERENCES [dbo].[Students]
        ([StudentId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Groups_Students'
CREATE INDEX [IX_FK_Groups_Students]
ON [dbo].[Groups]
    ([Elder]);
GO

-- Creating foreign key on [StudyForm] in table 'Groups'
ALTER TABLE [dbo].[Groups]
ADD CONSTRAINT [FK_Groups_StudyForms]
    FOREIGN KEY ([StudyForm])
    REFERENCES [dbo].[StudyForms]
        ([StudyFormId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Groups_StudyForms'
CREATE INDEX [IX_FK_Groups_StudyForms]
ON [dbo].[Groups]
    ([StudyForm]);
GO

-- Creating foreign key on [StudyGroup] in table 'Students'
ALTER TABLE [dbo].[Students]
ADD CONSTRAINT [FK_Students_Groups]
    FOREIGN KEY ([StudyGroup])
    REFERENCES [dbo].[Groups]
        ([GroupId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Students_Groups'
CREATE INDEX [IX_FK_Students_Groups]
ON [dbo].[Students]
    ([StudyGroup]);
GO

-- Creating foreign key on [MarkType] in table 'Marks'
ALTER TABLE [dbo].[Marks]
ADD CONSTRAINT [FK_Marks_MarkTypes]
    FOREIGN KEY ([MarkType])
    REFERENCES [dbo].[MarkTypes]
        ([MarkTypeId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Marks_MarkTypes'
CREATE INDEX [IX_FK_Marks_MarkTypes]
ON [dbo].[Marks]
    ([MarkType]);
GO

-- Creating foreign key on [Student] in table 'Marks'
ALTER TABLE [dbo].[Marks]
ADD CONSTRAINT [FK_Marks_Students]
    FOREIGN KEY ([Student])
    REFERENCES [dbo].[Students]
        ([StudentId])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Marks_Students'
CREATE INDEX [IX_FK_Marks_Students]
ON [dbo].[Marks]
    ([Student]);
GO

-- Creating foreign key on [SubjectInfo] in table 'Marks'
ALTER TABLE [dbo].[Marks]
ADD CONSTRAINT [FK_Marks_SubjectInfo]
    FOREIGN KEY ([SubjectInfo])
    REFERENCES [dbo].[SubjectInfo]
        ([SubjectInfoId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Marks_SubjectInfo'
CREATE INDEX [IX_FK_Marks_SubjectInfo]
ON [dbo].[Marks]
    ([SubjectInfo]);
GO

-- Creating foreign key on [QualificationLevel] in table 'SubjectInfo'
ALTER TABLE [dbo].[SubjectInfo]
ADD CONSTRAINT [FK_SubjectInfo_QualificationLevels]
    FOREIGN KEY ([QualificationLevel])
    REFERENCES [dbo].[QualificationLevels]
        ([QualificationLevelId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SubjectInfo_QualificationLevels'
CREATE INDEX [IX_FK_SubjectInfo_QualificationLevels]
ON [dbo].[SubjectInfo]
    ([QualificationLevel]);
GO

-- Creating foreign key on [StudyForm] in table 'SubjectInfo'
ALTER TABLE [dbo].[SubjectInfo]
ADD CONSTRAINT [FK_SubjectInfo_StudyForms]
    FOREIGN KEY ([StudyForm])
    REFERENCES [dbo].[StudyForms]
        ([StudyFormId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SubjectInfo_StudyForms'
CREATE INDEX [IX_FK_SubjectInfo_StudyForms]
ON [dbo].[SubjectInfo]
    ([StudyForm]);
GO

-- Creating foreign key on [SubjectName] in table 'SubjectInfo'
ALTER TABLE [dbo].[SubjectInfo]
ADD CONSTRAINT [FK_SubjectInfo_SubjectNames]
    FOREIGN KEY ([SubjectName])
    REFERENCES [dbo].[SubjectNames]
        ([SubjectNameId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SubjectInfo_SubjectNames'
CREATE INDEX [IX_FK_SubjectInfo_SubjectNames]
ON [dbo].[SubjectInfo]
    ([SubjectName]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------