ALTER TABLE [dbo].[Pieces]
ADD CONSTRAINT [F_Pieces_Composers]
FOREIGN KEY (Composer)
REFERENCES [Composers] (Id)
