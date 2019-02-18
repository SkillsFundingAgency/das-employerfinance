CREATE TABLE [dbo].[ProcessLevyDeclarationsJobs]
(
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [PayrollPeriod] DATETIME2 NOT NULL,
    [AccountPayeSchemeId] BIGINT NULL,
    [ImportLevyDeclarationsTasksCount] INT NOT NULL,
    [ImportLevyDeclarationsTasksCompletedCount] INT NOT NULL,
    [UpdateAccountBalanceTasksCount] INT NOT NULL,
    [UpdateAccountBalanceTasksCompletedCount] INT NOT NULL,
    [IsComplete] BIT NOT NULL,
    [Created] DATETIME2 NOT NULL,
    [Updated] DATETIME2 NULL,
    CONSTRAINT [PK_ProcessLevyDeclarationsJobs] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProcessLevyDeclarationsJobs_AccountPayeSchemes_AccountPayeSchemeId] FOREIGN KEY ([AccountPayeSchemeId]) REFERENCES [AccountPayeSchemes] ([Id]),
    CONSTRAINT [UK_ProcessLevyDeclarationsJobs_PayrollPeriod_AccountPayeSchemeId] UNIQUE ([PayrollPeriod] ASC, [AccountPayeSchemeId] ASC),
    INDEX [IX_ProcessLevyDeclarationsJobs_AccountPayeSchemeId] NONCLUSTERED ([AccountPayeSchemeId] ASC)
)