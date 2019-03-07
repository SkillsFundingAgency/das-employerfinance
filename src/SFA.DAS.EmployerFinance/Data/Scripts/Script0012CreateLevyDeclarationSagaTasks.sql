CREATE TABLE [dbo].[LevyDeclarationSagaTasks]
(
    [Id] BIGINT NOT NULL IDENTITY,
    [SagaId] INT NOT NULL,
    [Type] SMALLINT NOT NULL,
    [AccountPayeSchemeId] BIGINT NULL,
    [AccountId] BIGINT NULL,
    [Started] DATETIME2 NOT NULL,
    [Finished] DATETIME2 NOT NULL,
    CONSTRAINT [PK_LevyDeclarationSagaTasks] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_LevyDeclarationSagaTasks_LevyDeclarationSagas_SagaId] FOREIGN KEY ([SagaId]) REFERENCES [LevyDeclarationSagas] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_LevyDeclarationSagaTasks_AccountPayeSchemes_AccountPayeSchemeId] FOREIGN KEY ([AccountPayeSchemeId]) REFERENCES [AccountPayeSchemes] ([Id]),
    CONSTRAINT [FK_LevyDeclarationSagaTasks_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id]),
    CONSTRAINT [UK_LevyDeclarationSagaTasks_SagaId] UNIQUE ([SagaId] ASC, [Type] ASC, [AccountPayeSchemeId] ASC, [AccountId] ASC),
    INDEX [IX_LevyDeclarationSagaTasks_SagaId] NONCLUSTERED ([SagaId] ASC),
    INDEX [IX_LevyDeclarationSagaTasks_AccountPayeSchemeId] NONCLUSTERED ([AccountPayeSchemeId] ASC),
    INDEX [IX_LevyDeclarationSagaTasks_AccountId] NONCLUSTERED ([AccountId] ASC)
)