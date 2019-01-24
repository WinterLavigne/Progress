CREATE TABLE [dbo].[Pieces]
(
	[Id] [uniqueidentifier] NOT NULL PRIMARY KEY CONSTRAINT [D_Pieces_Id] DEFAULT NEWSEQUENTIALID(),
	[Name] [nvarchar](max) NOT NULL,
	[Created] [datetime2](7) NULL,
	[Composer] [uniqueidentifier] NULL

)
