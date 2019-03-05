CREATE TABLE [dbo].[LevyDeclarationSagas]
(
    [Id] INT NOT NULL IDENTITY,
    [Type] SMALLINT NOT NULL,
    [PayrollPeriod] DATETIME2 NOT NULL,
    [AccountPayeSchemeHighWaterMarkId] BIGINT NULL,
    [AccountPayeSchemeId] BIGINT NULL,
    [Created] DATETIME2 NOT NULL,
    [Updated] DATETIME2 NULL,
    [ImportPayeSchemeLevyDeclarationsTasksCount] INT NOT NULL,
    [ImportPayeSchemeLevyDeclarationsTasksCompleteCount] INT NOT NULL,
    [UpdateAccountTransactionBalancesTasksCount] INT NOT NULL,
    [UpdateAccountTransactionBalancesTasksCompleteCount] INT NOT NULL,
    [IsComplete] BIT NOT NULL,
    CONSTRAINT [PK_LevyDeclarationSagas] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_LevyDeclarationSagas_AccountPayeSchemes_AccountPayeSchemeHighWaterMarkId] FOREIGN KEY ([AccountPayeSchemeHighWaterMarkId]) REFERENCES [AccountPayeSchemes] ([Id]),
    CONSTRAINT [FK_LevyDeclarationSagas_AccountPayeSchemes_AccountPayeSchemeId] FOREIGN KEY ([AccountPayeSchemeId]) REFERENCES [AccountPayeSchemes] ([Id]),
    INDEX [IX_LevyDeclarationSagas_AccountPayeSchemeHighWaterMarkId] NONCLUSTERED ([AccountPayeSchemeHighWaterMarkId] ASC),
    INDEX [IX_LevyDeclarationSagas_AccountPayeSchemeId] NONCLUSTERED ([AccountPayeSchemeId] ASC)
)
GO

CREATE UNIQUE INDEX [UK_LevyDeclarationSagas_PayrollPeriod] ON [dbo].[LevyDeclarationSagas] ([PayrollPeriod] ASC) WHERE [Type] = 0
GO

CREATE UNIQUE INDEX [UK_LevyDeclarationSagas_PayrollPeriod_AccountPayeSchemeId] ON [dbo].[LevyDeclarationSagas] ([PayrollPeriod] ASC, [AccountPayeSchemeId] ASC) WHERE [Type] = 1
GO