CREATE TABLE [dbo].[ProcessLevyDeclarationsJobTasks]
(
    [Id] BIGINT NOT NULL IDENTITY,
    [JobId] UNIQUEIDENTIFIER NOT NULL,
    [Type] SMALLINT NOT NULL,
    [AccountPayeSchemeId] BIGINT NULL,
    [AccountId] BIGINT NULL,
    [Started] DATETIME2 NOT NULL,
    [Finished] DATETIME2 NOT NULL,
    CONSTRAINT [PK_ProcessLevyDeclarationsJobTasks] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProcessLevyDeclarationsJobTasks_ProcessLevyDeclarationsJobs_JobId] FOREIGN KEY ([JobId]) REFERENCES [ProcessLevyDeclarationsJobs] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ProcessLevyDeclarationsJobTasks_AccountPayeSchemes_AccountPayeSchemeId] FOREIGN KEY ([AccountPayeSchemeId]) REFERENCES [AccountPayeSchemes] ([Id]),
    CONSTRAINT [FK_ProcessLevyDeclarationsJobTasks_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id]),
    CONSTRAINT [UK_ProcessLevyDeclarationsJobTasks_JobId] UNIQUE ([JobId] ASC, [Type] ASC, [AccountPayeSchemeId] ASC, [AccountId] ASC),
    INDEX [IX_ProcessLevyDeclarationsJobTasks_JobId] NONCLUSTERED ([JobId] ASC),
    INDEX [IX_ProcessLevyDeclarationsJobTasks_AccountPayeSchemeId] NONCLUSTERED ([AccountPayeSchemeId] ASC),
    INDEX [IX_ProcessLevyDeclarationsJobTasks_AccountId] NONCLUSTERED ([AccountId] ASC)
)