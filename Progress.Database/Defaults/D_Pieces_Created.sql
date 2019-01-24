ALTER TABLE [dbo].[Pieces]
ADD CONSTRAINT [D_Pieces_Created]
DEFAULT (getdate())
FOR [Created]
