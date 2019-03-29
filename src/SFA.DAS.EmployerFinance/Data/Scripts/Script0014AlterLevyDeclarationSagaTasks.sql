ALTER TABLE [dbo].[LevyDeclarationSagaTasks] ALTER COLUMN [Finished] DATETIME2 NULL
ALTER TABLE [dbo].[LevyDeclarationSagaTasks] ADD [Errored] DATETIME2 NULL
ALTER TABLE [dbo].[LevyDeclarationSagaTasks] ADD [ErrorMessage] NVARCHAR(MAX) NULL