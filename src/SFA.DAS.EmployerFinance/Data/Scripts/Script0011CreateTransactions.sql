CREATE TABLE [dbo].[Transactions]
(
  [Id] BIGINT NOT NULL IDENTITY,
  [AccountId] BIGINT NOT NULL,
  [Type] TINYINT NOT NULL,
  [SubType] TINYINT NOT NULL,
  [Period] DATE NOT NULL,
  [Amount] MONEY,
  [Balance] MONEY,
  [JobId] UNIQUEIDENTIFIER,
  CONSTRAINT [PK_Transactions] PRIMARY KEY CLUSTERED ([Id] ASC),
  CONSTRAINT [FK_Transactions_Accounts_Id] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id]),
)