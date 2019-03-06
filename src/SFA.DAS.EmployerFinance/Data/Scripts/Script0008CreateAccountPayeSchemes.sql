CREATE TABLE [dbo].[AccountPayeSchemes]
(
  [Id] BIGINT NOT NULL IDENTITY,
  [AccountId] BIGINT NOT NULL,
  [EmployerReferenceNumber] VARCHAR(16) NOT NULL,
  [Created] DATETIME2 NOT NULL,
  [Deleted] DATETIME2 NULL,
  CONSTRAINT [PK_AccountPayeSchemes] PRIMARY KEY CLUSTERED ([Id] ASC),
  CONSTRAINT [FK_AccountPayeSchemes_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id]),
  INDEX [IX_AccountPayeSchemes_AccountId] NONCLUSTERED ([AccountId] ASC)
)
GO

CREATE UNIQUE NONCLUSTERED INDEX [AK_AccountPayeSchemes_AccountId_EmployerReferenceNumber] ON [AccountPayeSchemes] ([AccountId] ASC, [EmployerReferenceNumber] ASC) WHERE [Deleted] IS NULL
GO