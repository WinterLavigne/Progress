ALTER TABLE [dbo].[Composers]
ADD CONSTRAINT [D_Composers_Created]
DEFAULT (getdate())
FOR [Created]