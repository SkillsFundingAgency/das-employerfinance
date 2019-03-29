EXEC sp_rename '[dbo].[LevyDeclarationSagas].[ImportPayeSchemeLevyDeclarationsTasksCompleteCount]', '[ImportPayeSchemeLevyDeclarationsTasksFinishedCount]', 'COLUMN'
EXEC sp_rename '[dbo].[LevyDeclarationSagas].[UpdateAccountTransactionBalancesTasksCompleteCount]', '[UpdateAccountTransactionBalancesTasksFinishedCount]', 'COLUMN'
ALTER TABLE [dbo].[LevyDeclarationSagas] ADD [ImportPayeSchemeLevyDeclarationsTasksErroredCount] INT NOT NULL CONSTRAINT [DC_ImportPayeSchemeLevyDeclarationsTasksErroredCount] DEFAULT 0
ALTER TABLE [dbo].[LevyDeclarationSagas] ADD [UpdateAccountTransactionBalancesTasksErroredCount] INT NOT NULL CONSTRAINT [DC_UpdateAccountTransactionBalancesTasksErroredCount] DEFAULT 0
ALTER TABLE [dbo].[LevyDeclarationSagas] DROP CONSTRAINT [DC_ImportPayeSchemeLevyDeclarationsTasksErroredCount]
ALTER TABLE [dbo].[LevyDeclarationSagas] DROP CONSTRAINT [DC_UpdateAccountTransactionBalancesTasksErroredCount]