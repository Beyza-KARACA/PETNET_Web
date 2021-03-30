CREATE TABLE [dbo].[Animals] (
    [AnimalID] INT           IDENTITY (1, 1) NOT NULL,
    [UserID]   INT           NOT NULL,
    [Name]     NVARCHAR (50) NULL,
    [Gender]   NVARCHAR (10) NULL,
    [Type]     NVARCHAR (50) NULL,
    [Kind]     NVARCHAR (50) NULL,
    [Photo]    NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Animals] PRIMARY KEY CLUSTERED ([AnimalID] ASC),
    CONSTRAINT [FK_Animals_Users] FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserID])
);

