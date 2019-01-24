CREATE TABLE [dbo].[Composers]
(
	[Id] [uniqueidentifier] NOT NULL PRIMARY KEY CONSTRAINT [D_Composers_Id] DEFAULT NEWSEQUENTIALID(),
	[Name] [nvarchar](max) NOT NULL,
	[Created] [datetime2](7) NULL,
)
