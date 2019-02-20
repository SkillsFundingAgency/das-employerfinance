CREATE TABLE [dbo].[PayeSchemes]
(
  [Id] BIGINT NOT NULL IDENTITY,
  [EmployerReferenceNumber] VARCHAR(16) NOT NULL,
  [AccountId] BIGINT NOT NULL,
  [Name] NVARCHAR(60) NULL, -- todo: check source for >60
  [Created] DATETIME2 NOT NULL,
  [Updated] DATETIME2 NULL,
  [Deleted] DATETIME2 NULL,
  CONSTRAINT [PK_PayeSchemes] PRIMARY KEY NONCLUSTERED ([EmployerReferenceNumber] ASC),
  CONSTRAINT [AK_PayeSchemes_Id] UNIQUE CLUSTERED ([Id] ASC),
  CONSTRAINT [FK_PayeSchemes_Accounts_AccountID] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id]),
  INDEX [IX_PayeSchemes_AccountID] NONCLUSTERED ([AccountId] ASC)
)
