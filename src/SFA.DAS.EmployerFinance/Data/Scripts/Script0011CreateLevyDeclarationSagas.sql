CREATE TABLE [dbo].[LevyDeclarationSagas]
(
    [Id] INT NOT NULL IDENTITY,
    [Type] SMALLINT NOT NULL,
    [PayrollPeriod] DATETIME2 NOT NULL,
    [HighWaterMarkId] BIGINT NOT NULL,
    [Created] DATETIME2 NOT NULL,
    [Updated] DATETIME2 NULL,
    [ImportPayeSchemeLevyDeclarationsTasksCount] INT NOT NULL,
    [ImportPayeSchemeLevyDeclarationsTasksStarted] DATETIME2 NULL,
    [ImportPayeSchemeLevyDeclarationsTasksCompleteCount] INT NOT NULL,
    [ImportPayeSchemeLevyDeclarationsTasksFinished] DATETIME2 NULL,
    [UpdateAccountTransactionBalancesTasksCount] INT NOT NULL,
    [UpdateAccountTransactionBalancesTasksStarted] DATETIME2 NULL,
    [UpdateAccountTransactionBalancesTasksCompleteCount] INT NOT NULL,
    [UpdateAccountTransactionBalancesTasksFinished] DATETIME2 NULL,
    [IsComplete] BIT NOT NULL,
    CONSTRAINT [PK_LevyDeclarationSagas] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_LevyDeclarationSagas_AccountPayeSchemes_HighWaterMarkId] FOREIGN KEY ([HighWaterMarkId]) REFERENCES [AccountPayeSchemes] ([Id])
)
GO

CREATE UNIQUE INDEX [UK_LevyDeclarationSagas_PayrollPeriod] ON [dbo].[LevyDeclarationSagas] ([PayrollPeriod] ASC) WHERE [Type] = 0
GO

CREATE UNIQUE INDEX [UK_LevyDeclarationSagas_PayrollPeriod_HighWaterMarkId] ON [dbo].[LevyDeclarationSagas] ([PayrollPeriod] ASC, [HighWaterMarkId] ASC) WHERE [Type] = 1
GO