CREATE TABLE [dbo].[AccountPayeSchemes]
(
  [Id] BIGINT NOT NULL IDENTITY,
  [AccountId] BIGINT NOT NULL,
  [EmployerReferenceNumber] VARCHAR(16) NOT NULL,
  [Created] DATETIME2 NOT NULL,
  [Updated] DATETIME2 NULL,
  [Deleted] DATETIME2 NULL,
  CONSTRAINT [PK_AccountPayeSchemes] PRIMARY KEY CLUSTERED ([Id] ASC),
  CONSTRAINT [FK_AccountPayeSchemes_Account_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id]),
  CONSTRAINT [FK_AccountPayeSchemes_PayeSchemes_EmployerReferenceNumber] FOREIGN KEY ([EmployerReferenceNumber]) REFERENCES [PayeSchemes] ([EmployerReferenceNumber]),
  CONSTRAINT [UK_AccountPayeSchemes_AccountId_EmployerReferenceNumber] UNIQUE ([AccountId] ASC, [EmployerReferenceNumber] ASC),
  INDEX [IX_AccountPayeSchemes_AccountId] NONCLUSTERED ([AccountId] ASC),
  INDEX [IX_AccountPayeSchemes_EmployerReferenceNumber] NONCLUSTERED ([EmployerReferenceNumber] ASC)
)
