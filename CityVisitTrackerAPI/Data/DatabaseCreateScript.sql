
USE [CityVisitTrackerAPIContext]
GO
--Create StateTable
/****** Object: Table [dbo].[State] Script Date: 3/8/2019 4:46:09 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[State] (
    [StateId]      INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (MAX) NULL,
    [Abbreviation] NVARCHAR (MAX) NULL,
    [DateAdded]    DATETIME2 (7)  NOT NULL,
    [LastUpdated]  DATETIME2 (7)  NOT NULL,
	    CONSTRAINT [PK_State] PRIMARY KEY CLUSTERED ([StateId] ASC),
);

-- Create City Table
CREATE TABLE [dbo].[City] (
    [CityId]      INT             IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (MAX)  NULL,
    [StateId]     INT             NOT NULL,
    [Latitude]    DECIMAL (18, 2) NULL,
    [Longitude]   DECIMAL (18, 2) NULL,
    [DateAdded]   DATETIME2 (7)   NOT NULL,
    [LastUpdated] DATETIME2 (7)   NOT NULL,
    CONSTRAINT [PK_City] PRIMARY KEY CLUSTERED ([CityId] ASC),
    CONSTRAINT [FK_City_State_StateId] FOREIGN KEY ([StateId]) REFERENCES [dbo].[State] ([StateId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_City_StateId]
    ON [dbo].[City]([StateId] ASC);
-- Create UserTable
CREATE TABLE [dbo].[User] (
    [UserId]       INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]    NVARCHAR (MAX) NULL,
    [LastName]     NVARCHAR (MAX) NULL,
    [UserName]     NVARCHAR (MAX) NULL,
    [PasswordHash] NVARCHAR (MAX) NULL,
    [PasswordSalt] NVARCHAR (MAX) NULL,
    [DateAdded]    DATETIME2 (7)  NOT NULL,
    [LastUpdated]  DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([UserId] ASC)
);
-- Create UserCityVisits table
CREATE TABLE [dbo].[UserCityVisits] (
    [UserId] INT NOT NULL,
    [CityId] INT NOT NULL,
    CONSTRAINT [PK_UserCityVisits] PRIMARY KEY CLUSTERED ([CityId] ASC, [UserId] ASC),
    CONSTRAINT [FK_UserCityVisits_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_UserCityVisits_UserId]
    ON [dbo].[UserCityVisits]([UserId] ASC);



